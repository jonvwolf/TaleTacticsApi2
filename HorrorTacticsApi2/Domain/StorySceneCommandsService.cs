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

        public async Task<ReadStorySceneCommandModel?> TryGetAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await FindCommand(id, includeAll, token);
            return entity == default ? default : handler.CreateReadModel(entity);
        }

        public async Task<List<ReadStorySceneCommandModel>> GetAllAsync(long storySceneId, bool includeAll, CancellationToken token)
        {
            var entities = await FindCommandsAsync(includeAll, storySceneId, token);
            return handler.CreateReadModel(entities);
        }

        public async Task<ReadStorySceneCommandModel> CreateCommandAsync(long sceneId, CreateStorySceneCommandModel model, bool basicValidated, CancellationToken token)
        {
            var scene = await scenes.FindStorySceneAsync(sceneId, true, token);
            if (scene == default)
                throw new HtNotFoundException($"Scene with id {sceneId} not found");

            handler.Validate(model, basicValidated);
            var entity = await handler.CreateEntityAsync(model, scene, token);
            context.StorySceneCommands.Add(entity);
            await context.SaveChangesWrappedAsync(token);

            return handler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneCommandModel> UpdateCommandAsync(long id, UpdateStorySceneCommandModel model, bool basicValidated, CancellationToken token)
        {
            var entity = await FindCommand(id, true, token);
            if (entity == default)
                throw new HtNotFoundException($"Command with Id {id} not found");

            handler.Validate(model, basicValidated);
            await handler.UpdateEntityAsync(model, entity, token);

            await context.SaveChangesWrappedAsync(token);

            return handler.CreateReadModel(entity);
        }

        public async Task DeleteCommandAsync(long id, CancellationToken token)
        {
            var entity = await FindCommand(id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"Command with Id {id} not found");

            context.StorySceneCommands.Remove(entity);
            await context.SaveChangesWrappedAsync(token);
        }

        async Task<StorySceneCommandEntity?> FindCommand(long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        async Task<List<StorySceneCommandEntity>> FindCommandsAsync(bool includeAll, long storySceneId, CancellationToken token)
        {
            var entity = await GetQuery(includeAll).Where(x => x.ParentStoryScene.Id == storySceneId).ToListAsync(token);

            return entity;
        }

        IQueryable<StorySceneCommandEntity> GetQuery(bool includeAll = true)
        {
            IQueryable<StorySceneCommandEntity> query = context.StorySceneCommands;

            if (includeAll)
            {
                // TODO: this should be organized (code)
                query = query
                        .Include(x => x.ParentStoryScene)
                        .ThenInclude(x => x.ParentStory)
                    ;
            }
            return query;
        }
    }
}
