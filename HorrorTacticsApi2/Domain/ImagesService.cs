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

        public ImagesService(IHorrorDbContext context, ImageModelEntityHandler handler, FileUploadHandler fileUploadHandler)
        {
            _context = context;
            _imeHandler = handler;
            _fileUploadHandler = fileUploadHandler;
        }

        public async Task<IList<ReadImageModel>> GetAllImagesAsync(CancellationToken token)
        {
            var list = new List<ReadImageModel>();
            var images = await GetQuery().ToListAsync(token);
            images.ForEach(image => { list.Add(_imeHandler.CreateReadModel(image)); });

            return list;
        }

        public async Task<ReadImageModel?> TryGetAsync(long id, CancellationToken token)
        {
            var entity = await TryFindImageAsync(id, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadImageModel> CreateImageAsync(CancellationToken token)
        {
            var uploadedFile = await _fileUploadHandler.HandleAsync(FormatHelper.AllowedImageExtensionsForUpload, token);

            ImageEntity entity;
            try
            {
                entity = _imeHandler.CreateEntity(uploadedFile);

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

        public async Task<ReadImageModel> UpdateImageAsync(long id, UpdateImageModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);
            var entity = await TryFindImageAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteImageAsync(long id, CancellationToken token)
        {
            var entity = await TryFindImageAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _fileUploadHandler.DeleteUploadedFile(entity.File.Filename);

            _context.Images.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
        }

        public async Task<ImageEntity?> TryFindImageAsync(long id, CancellationToken token)
        {
            var entity = await GetQuery().SingleOrDefaultAsync(x => x.Id == id, token);
            return entity;
        }

        IQueryable<ImageEntity> GetQuery(bool includeFiles = true)
        {
            IQueryable<ImageEntity> query = _context.Images;

            if (includeFiles)
                query = query.Include(x => x.File);

            return query;
        }
    }
}
