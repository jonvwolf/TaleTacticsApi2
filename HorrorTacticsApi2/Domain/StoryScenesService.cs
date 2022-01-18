using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class StoryScenesService
    {
        readonly IHorrorDbContext _context;
        readonly StorySceneModelEntityHandler _imeHandler;
        
        public StoryScenesService(IHorrorDbContext context, StorySceneModelEntityHandler handler)
        {
            _context = context;
            _imeHandler = handler;
        }

        public async Task<IList<ReadStorySceneModel>> GetAllStoryScenesAsync(CancellationToken token)
        {
            var list = new List<ReadStorySceneModel>();
            var stories = await GetQuery(true).ToListAsync(token);
            stories.ForEach(story => { list.Add(_imeHandler.CreateReadModel(story)); });

            return list;
        }

        public async Task<ReadStorySceneModel?> TryGetAsync(long id, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(id, true, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneModel> CreateStorySceneAsync(CreateStorySceneModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);

            var entity = _imeHandler.CreateEntityAsync(model);
            _context.StoryScenes.Add(entity);
            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneModel> UpdateStorySceneAsync(long id, UpdateStorySceneModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);
            var entity = await FindStorySceneAsync(id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"Story with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteStorySceneAsync(long id, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"Story with Id {id} not found");

            _context.StoryScenes.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
        }

        async Task<StorySceneEntity?> FindStorySceneAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        IQueryable<StorySceneEntity> GetQuery(bool includeAll = true)
        {
            IQueryable<StorySceneEntity> query = _context.StoryScenes;

            if (includeAll)
            {
                // TODO: this should be organized (code)
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                query = query
                        .Include(x => x.Audios)
                            .ThenInclude(x => x.File)
                        .Include(x => x.Images)
                            .ThenInclude(x => x.File);
                    ;
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            }
            return query;
        }
    }
}
