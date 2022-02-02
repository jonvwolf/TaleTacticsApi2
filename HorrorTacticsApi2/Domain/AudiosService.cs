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

        public AudiosService(IHorrorDbContext context, AudioModelEntityHandler handler, FileUploadHandler fileUploadHandler)
        {
            _context = context;
            _imeHandler = handler;
            _fileUploadHandler = fileUploadHandler;
        }

        public async Task<IList<ReadAudioModel>> GetAllAudiosAsync(CancellationToken token)
        {
            var list = new List<ReadAudioModel>();
            var images = await GetQuery().ToListAsync(token);
            images.ForEach(image => { list.Add(_imeHandler.CreateReadModel(image)); });

            return list;
        }

        public async Task<ReadAudioModel?> TryGetAsync(long id, CancellationToken token)
        {
            var entity = await TryFindAudioAsync(id, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<Stream> GetStreamFileAsync(string filename, bool isBasicValidated, CancellationToken token)
        {
            if (!isBasicValidated)
                throw new NotImplementedException();

            var entity = await TryFindAudioAsync(filename, token);
            if (entity == default)
                throw new HtNotFoundException("File not found");

            // this is fine as long as it is validated against database value
            var stream = _fileUploadHandler.GetFileStream(filename);

            return stream;
        }

        public async Task<ReadAudioModel> CreateAudioAsync(CancellationToken token)
        {
            var uploadedFile = await _fileUploadHandler.HandleAsync(FormatHelper.AllowedAudioExtensionsForUpload, token);

            AudioEntity entity;
            try
            {
                entity = _imeHandler.CreateEntity(uploadedFile);

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

        public async Task<ReadAudioModel> UpdateAudioAsync(long id, UpdateAudioModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);
            var entity = await TryFindAudioAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteAudioAsync(long id, CancellationToken token)
        {
            var entity = await TryFindAudioAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            string filename = entity.File.Filename;

            using var transaction = await _context.CreateTransactionAsync();

            // TODO: repeated code in Images
            _context.Files.Remove(entity.File);
            _context.Audios.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
            await transaction.CommitAsync(token);

            // TODO: move physical file to a trash folder, if entity fails to be removed, bring back the file
            _fileUploadHandler.DeleteUploadedFile(filename);
        }

        public async Task<AudioEntity?> TryFindAudioAsync(long id, CancellationToken token)
        {
            var entity = await GetQuery().SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        public async Task<AudioEntity?> TryFindAudioAsync(string filename, CancellationToken token)
        {
            var entity = await GetQuery().SingleOrDefaultAsync(x => x.File.Filename == filename, token);

            return entity;
        }

        IQueryable<AudioEntity> GetQuery(bool includeFiles = true)
        {
            IQueryable<AudioEntity> query = _context.Audios;

            if (includeFiles)
                query = query.Include(x => x.File);

            return query;
        }
    }
}
