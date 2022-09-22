using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Audio;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class AudiosService
    {
        readonly IHorrorDbContext _context;
        readonly AudioModelEntityHandler _imeHandler;
        readonly FileUploadHandler _fileUploadHandler;
        readonly UserService _user;

        public AudiosService(IHorrorDbContext context, AudioModelEntityHandler handler, FileUploadHandler fileUploadHandler, UserService user)
        {
            _context = context;
            _imeHandler = handler;
            _fileUploadHandler = fileUploadHandler;
            _user = user;
        }

        public async Task<IList<ReadAudioModel>> GetAllAudiosAsync(UserJwt user, CancellationToken token)
        {
            var list = new List<ReadAudioModel>();
            var images = await GetQuery(user.Id).ToListAsync(token);
            images.ForEach(image => { list.Add(_imeHandler.CreateReadModel(image)); });

            return list;
        }

        public async Task<ReadAudioModel?> TryGetAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await TryFindAudioAsync(user.Id, id, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<Stream> GetStreamFileAsync(string filename, bool isBasicValidated, CancellationToken token)
        {
            if (!isBasicValidated)
                throw new NotImplementedException();

            var entity = await TryFindAudioAsync(default, filename, token);
            if (entity == default)
                throw new HtNotFoundException("File not found");

            // this is fine as long as it is validated against database value
            var stream = _fileUploadHandler.GetFileStream(filename, entity.File.IsDefault);

            return stream;
        }

        public async Task<ReadAudioModel> CreateAudioAsync(UserJwt user, CancellationToken token)
        {
            var uploadedFile = await _fileUploadHandler.HandleAsync(FormatHelper.AllowedAudioExtensionsForUpload, token);

            AudioEntity entity;
            try
            {
                entity = _imeHandler.CreateEntity(_user.GetReference(user), uploadedFile);

                _context.Audios.Add(entity);
                await _context.SaveChangesWrappedAsync(token);
            }
            catch (Exception)
            {
                // TODO: repeated code in ImageService
                // TODO: background job to check orphan files...
                _fileUploadHandler.TryDeleteUploadedFile(uploadedFile);
                throw;
            }
            
            return _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadAudioModel> ReplaceAudioFileAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await TryFindAudioAsync(user.Id, id, token);
            if (entity == default)
                throw new HtNotFoundException($"Audio with Id {id} not found");

            var uploadedFile = await _fileUploadHandler.HandleAsync(FormatHelper.AllowedAudioExtensionsForUpload, token);

            string? oldFileToDelete;

            try
            {
                oldFileToDelete = _imeHandler.UpdateEntity(entity, uploadedFile);
                await _context.SaveChangesWrappedAsync(token);
            }
            catch (Exception)
            {
                _fileUploadHandler.TryDeleteUploadedFile(uploadedFile);
                throw;
            }

            if (oldFileToDelete != default)
                _fileUploadHandler.TryDeleteUploadedFile(oldFileToDelete);

            return _imeHandler.CreateReadModel(entity);
            
        }

        public async Task<ReadAudioModel> UpdateAudioAsync(UserJwt user, long id, UpdateAudioModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);
            var entity = await TryFindAudioAsync(user.Id, id, token);
            if (entity == default)
                throw new HtNotFoundException($"Audio with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteAudioAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await TryFindAudioAsync(user.Id, id, token);
            if (entity == default)
                throw new HtNotFoundException($"Audio with Id {id} not found");

            string filename = entity.File.Filename;
            bool isDefault = entity.File.IsDefault;

            using var transaction = await _context.CreateTransactionAsync();

            // TODO: repeated code in Images
            _context.Files.Remove(entity.File);
            _context.Audios.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
            await transaction.CommitAsync(token);

            if (!isDefault)
            {
                // TODO: move physical file to a trash folder, if entity fails to be removed, bring back the file
                _fileUploadHandler.DeleteUploadedFile(filename);
            }
        }

        public async Task<AudioEntity?> TryFindAudioAsync(long userId, long id, CancellationToken token)
        {
            var entity = await GetQuery(userId).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        public async Task<AudioEntity?> TryFindAudioAsync(long? userId, string filename, CancellationToken token)
        {
            var entity = await GetQuery(userId).SingleOrDefaultAsync(x => x.File.Filename == filename, token);

            return entity;
        }

        IQueryable<AudioEntity> GetQuery(long? userId, bool includeFiles = true)
        {
            IQueryable<AudioEntity> query = _context.Audios;

            if (userId.HasValue)
                query = query.Where(x => x.File.Owner.Id == userId);

            if (includeFiles)
                query = query.Include(x => x.File);

            return query;
        }
    }
}
