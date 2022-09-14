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

        public async Task<ReadStorySceneModel?> TryGetAsync(UserJwt user, long id, bool includeAll, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(user.Id, id, includeAll, token);
            return entity == default ? default : imeHandler.CreateReadModel(entity);
        }

        public async Task<List<ReadStorySceneModel>> GetAllAsync(UserJwt user, long storyId, bool includeAll, CancellationToken token)
        {
            var list = await GetQuery(user.Id, includeAll)
                .Where(x => x.ParentStory.Id == storyId)
                .OrderBy(x => x.Title)
                .Select(x => imeHandler.CreateReadModel(x))
                .ToListAsync(token);

            return list;
        }

        public async Task<ReadStorySceneModel> CreateStorySceneAsync(UserJwt user, long storyId, CreateStorySceneModel model, bool basicValidated, CancellationToken token)
        {
            var story = await stories.TryFindStoryAsync(user.Id, storyId, true, token);
            if (story == default)
                throw new HtNotFoundException($"Story with id {storyId} not found");

            var entity = imeHandler.CreateEntity(model, story);
            context.StoryScenes.Add(entity);
            await context.SaveChangesWrappedAsync(token);

            return imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneModel> UpdateStorySceneAsync(UserJwt user, long id, UpdateStorySceneModel model, bool basicValidated, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(user.Id, id, true, token);
            if (entity == default)
                throw new HtNotFoundException($"StoryScene with Id {id} not found");

            imeHandler.UpdateEntity(model, entity);

            await context.SaveChangesWrappedAsync(token);

            return imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteStorySceneAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await FindStorySceneAsync(user.Id, id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"StoryScene with Id {id} not found");

            context.StoryScenes.Remove(entity);
            await context.SaveChangesWrappedAsync(token);
        }

        public async Task<StorySceneEntity?> FindStorySceneAsync(long? userId, long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(userId, includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        IQueryable<StorySceneEntity> GetQuery(long? userId, bool includeAll = true)
        {
            IQueryable<StorySceneEntity> query = context.StoryScenes;

            if (userId.HasValue)
                query = query.Where(x => x.ParentStory.Owner.Id == userId);

            if (includeAll)
            {
                // TODO: this should be organized (code)
                query = query
                        .Include(x => x.Commands)
                            .ThenInclude(x => x.Audios)
                                .ThenInclude(x => x.File)
                        .Include(x => x.Commands)
                            .ThenInclude(x => x.Images)
                                .ThenInclude(x => x.File);
                ;
            }
            return query;
        }
    }
}
