using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class ImageService
    {
        readonly IHorrorDbContext _context;
        readonly ImageModelEntityConverter _converter;
        public ImageService(IHorrorDbContext context, ImageModelEntityConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public async Task<IList<ReadImageModel>> GetAllImagesAsync(CancellationToken token)
        {
            var list = new List<ReadImageModel>();
            var images = await _context.Images.ToListAsync(token);
            images.ForEach(image => { list.Add(_converter.CreateModel(image)); });

            return list;
        }

        public async Task<ReadImageModel?> GetAsync(long id, CancellationToken token)
        {
            var entity = await FindImageAsync(id, token);
            return entity == default ? default : _converter.CreateModel(entity);
        }

        public async Task<ReadImageModel> CreateImageAsync(CreateImageModel model, bool basicValidated, CancellationToken token)
        {
            _converter.Validate(model, basicValidated);
            var entity = _converter.CreateEntity(model);

            _context.Images.Add(entity);
            await _context.SaveChangesWrappedAsync(token);

            return _converter.CreateModel(entity);
        }

        public async Task<ReadImageModel> UpdateImageAsync(long id, UpdateImageModel model, bool basicValidated, CancellationToken token)
        {
            _converter.Validate(model, basicValidated);
            var entity = await FindImageAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _converter.SetEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _converter.CreateModel(entity);
        }

        public async Task DeleteImageAsync(long id, CancellationToken token)
        {
            var entity = await FindImageAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _context.Images.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
        }

        async Task<Image?> FindImageAsync(long id, CancellationToken token)
        {
            var entity = await _context.Images.SingleOrDefaultAsync(x => x.Id == id, token);
           
            return entity;
        }
    }
}
