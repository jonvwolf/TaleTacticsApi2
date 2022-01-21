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
        readonly IHorrorDbContext context;
        readonly StorySceneModelEntityHandler imeHandler;
        readonly StoriesService stories;

        public StoryScenesService(IHorrorDbContext context, StorySceneModelEntityHandler handler, StoriesService stories)
        {
            this.context = context;
            imeHandler = handler;
            this.stories = stories;
        }

        public async Task<ReadStorySceneModel?> TryGetAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(id, includeAll, token);
            return entity == default ? default : imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneModel> CreateStorySceneAsync(long storyId, CreateStorySceneModel model, bool basicValidated, CancellationToken token)
        {
            imeHandler.Validate(model, basicValidated);

            var story = await stories.TryFindStoryAsync(storyId, true, token);
            if (story == default)
                throw new HtNotFoundException($"Story with id {storyId} not found");

            var entity = await imeHandler.CreateEntityAsync(model, story, token);
            context.StoryScenes.Add(entity);
            await context.SaveChangesWrappedAsync(token);

            return imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneModel> UpdateStorySceneAsync(long id, UpdateStorySceneModel model, bool basicValidated, CancellationToken token)
        {
            imeHandler.Validate(model, basicValidated);
            var entity = await FindStorySceneAsync(id, true, token);
            if (entity == default)
                throw new HtNotFoundException($"StoryScene with Id {id} not found");

            await imeHandler.UpdateEntityAsync(model, entity, token);

            await context.SaveChangesWrappedAsync(token);

            return imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteStorySceneAsync(long id, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"StoryScene with Id {id} not found");

            context.StoryScenes.Remove(entity);
            await context.SaveChangesWrappedAsync(token);
        }

        async Task<StorySceneEntity?> FindStorySceneAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        IQueryable<StorySceneEntity> GetQuery(bool includeAll = true)
        {
            IQueryable<StorySceneEntity> query = context.StoryScenes;

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
