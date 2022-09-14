using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Handlers;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class StorySceneCommandsService
    {
        readonly IHorrorDbContext context;
        readonly StorySceneCommandModelEntityHandler handler;
        readonly StoryScenesService scenes;

        public StorySceneCommandsService(IHorrorDbContext context, StorySceneCommandModelEntityHandler handler, StoryScenesService scenes)
        {
            this.context = context;
            this.handler = handler;
            this.scenes = scenes;
        }

        public async Task<ReadStorySceneCommandModel?> TryGetAsync(UserJwt user, long id, bool includeAll, CancellationToken token)
        {
            var entity = await FindCommandAsync(user.Id, id, includeAll, token);
            return entity == default ? default : handler.CreateReadModel(entity);
        }

        public async Task<List<ReadStorySceneCommandModel>> GetAllAsync(UserJwt user, long storySceneId, bool includeAll, CancellationToken token)
        {
            var entities = await FindCommandsAsync(user.Id, includeAll, storySceneId, token);
            return handler.CreateReadModel(entities);
        }

        public async Task<ReadStorySceneCommandModel> CreateCommandAsync(UserJwt user, long sceneId, CreateStorySceneCommandModel model, bool basicValidated, CancellationToken token)
        {
            var scene = await scenes.FindStorySceneAsync(user.Id, sceneId, true, token);
            if (scene == default)
                throw new HtNotFoundException($"Scene with id {sceneId} not found");

            handler.Validate(model, basicValidated);
            var entity = await handler.CreateEntityAsync(user, model, scene, token);
            context.StorySceneCommands.Add(entity);
            await context.SaveChangesWrappedAsync(token);

            return handler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneCommandModel> UpdateCommandAsync(UserJwt user, long id, UpdateStorySceneCommandModel model, bool basicValidated, CancellationToken token)
        {
            var entity = await FindCommandAsync(user.Id, id, true, token);
            if (entity == default)
                throw new HtNotFoundException($"Command with Id {id} not found");

            handler.Validate(model, basicValidated);
            await handler.UpdateEntityAsync(user, model, entity, token);

            await context.SaveChangesWrappedAsync(token);

            return handler.CreateReadModel(entity);
        }

        public async Task DeleteCommandAsync(UserJwt user, long id, CancellationToken token)
        {
            var entity = await FindCommandAsync(user.Id, id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"Command with Id {id} not found");

            context.StorySceneCommands.Remove(entity);
            await context.SaveChangesWrappedAsync(token);
        }

        async Task<StorySceneCommandEntity?> FindCommandAsync(long? userId, long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(userId, includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        async Task<List<StorySceneCommandEntity>> FindCommandsAsync(long? userId, bool includeAll, long storySceneId, CancellationToken token)
        {
            var entity = await GetQuery(userId, includeAll).Where(x => x.ParentStoryScene.Id == storySceneId).ToListAsync(token);

            return entity;
        }

        IQueryable<StorySceneCommandEntity> GetQuery(long? userId, bool includeAll = true)
        {
            IQueryable<StorySceneCommandEntity> query = context.StorySceneCommands;
            if (userId.HasValue)
                query = query.Where(x => x.ParentStoryScene.ParentStory.Owner.Id == userId);
            if (includeAll)
            {
                // TODO: this should be organized (code)
                query = query
                        .Include(x => x.ParentStoryScene).ThenInclude(x => x.ParentStory)
                        .Include(x => x.Audios).ThenInclude(x => x.File)
                        .Include(x => x.Images).ThenInclude(x => x.File)
                    ;
            }
            return query;
        }
    }
}
