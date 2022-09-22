using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class ImagesService
    {
        readonly IHorrorDbContext _context;
        readonly ImageModelEntityHandler _imeHandler;
        readonly FileUploadHandler _fileUploadHandler;
        readonly UserService _user;
        public ImagesService(IHorrorDbContext context, ImageModelEntityHandler handler, FileUploadHandler fileUploadHandler, UserService user)
        {
            _user = user;
            _context = context;
            _imeHandler = handler;
            _fileUploadHandler = fileUploadHandler;
        }

        public async Task<IList<ReadImageModel>> GetAllImagesAsync(UserJwt user, CancellationToken token)
        {
            var list = new List<ReadImageModel>();
            var images = await GetQuery(user.Id).ToListAsync(token);
            images.ForEach(image => { list.Add(_imeHandler.CreateReadModel(image)); });

            return list;
        }

        public async Task<ReadImageModel?> TryGetAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await TryFindImageAsync(user.Id, id, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<Stream> GetStreamFileAsync(string filename, bool isBasicValidated, CancellationToken token)
        {
            if (!isBasicValidated)
                throw new NotImplementedException();

            var entity = await TryFindImageAsync(default, filename, token);
            if (entity == default)
                throw new HtNotFoundException("File not found");

            // this is fine as long as it is validated against database value
            var stream = _fileUploadHandler.GetFileStream(filename, entity.File.IsDefault);

            return stream;
        }

        public async Task<ReadImageModel> CreateImageAsync(UserJwt user, CancellationToken token)
        {
            var uploadedFile = await _fileUploadHandler.HandleAsync(FormatHelper.AllowedImageExtensionsForUpload, token);

            ImageEntity entity;
            try
            {
                entity = _imeHandler.CreateEntity(_user.GetReference(user), uploadedFile);

                _context.Images.Add(entity);
                await _context.SaveChangesWrappedAsync(token);
            }
            catch (Exception)
            {
                // TODO: repeated code in AudioService
                // TODO: background job to check orphan files...
                _fileUploadHandler.TryDeleteUploadedFile(uploadedFile);
                throw;
            }
            
            return _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadImageModel> ReplaceImageFileAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await TryFindImageAsync(user.Id, id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            var uploadedFile = await _fileUploadHandler.HandleAsync(FormatHelper.AllowedImageExtensionsForUpload, token);

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

        public async Task<ReadImageModel> UpdateImageAsync(UserJwt user, long id, UpdateImageModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);
            var entity = await TryFindImageAsync(user.Id, id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteImageAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await TryFindImageAsync(user.Id, id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            bool isDefault = entity.File.IsDefault;
            string filename = entity.File.Filename;

            using var transaction = await _context.CreateTransactionAsync();

            // TODO: repeated code in Audio
            _context.Files.Remove(entity.File);
            _context.Images.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
            await transaction.CommitAsync(token);

            if (!isDefault)
            {
                // TODO: move physical file to a trash folder, if entity fails to be removed, bring back the file
                _fileUploadHandler.DeleteUploadedFile(filename);
            }
        }

        public async Task<ImageEntity?> TryFindImageAsync(long? userId, long id, CancellationToken token)
        {
            var entity = await GetQuery(userId).SingleOrDefaultAsync(x => x.Id == id, token);
            return entity;
        }

        public async Task<ImageEntity?> TryFindImageAsync(long? userId, string filename, CancellationToken token)
        {
            var entity = await GetQuery(userId).SingleOrDefaultAsync(x => x.File.Filename == filename, token);

            return entity;
        }

        IQueryable<ImageEntity> GetQuery(long? userId, bool includeFiles = true)
        {
            IQueryable<ImageEntity> query = _context.Images;

            if (userId.HasValue)
                query = query.Where(x => x.File.Owner.Id == userId);

            if (includeFiles)
                query = query.Include(x => x.File);

            return query;
        }
    }
}
