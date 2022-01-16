using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Audio;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class AudioService
    {
        readonly IHorrorDbContext _context;
        readonly AudioModelEntityHandler _imeHandler;
        readonly FileUploadHandler _fileUploadHandler;

        public AudioService(IHorrorDbContext context, AudioModelEntityHandler handler, FileUploadHandler fileUploadHandler)
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

        public async Task<ReadAudioModel?> GetAsync(long id, CancellationToken token)
        {
            var entity = await FindAudioAsync(id, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadAudioModel> CreateImageAsync(CancellationToken token)
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
            var entity = await FindAudioAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteImageAsync(long id, CancellationToken token)
        {
            var entity = await FindAudioAsync(id, token);
            if (entity == default)
                throw new HtNotFoundException($"Image with Id {id} not found");

            _fileUploadHandler.DeleteUploadedFile(entity.File.Filename);

            _context.Audios.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
        }

        async Task<AudioEntity?> FindAudioAsync(long id, CancellationToken token)
        {
            var entity = await GetQuery().SingleOrDefaultAsync(x => x.Id == id, token);

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
