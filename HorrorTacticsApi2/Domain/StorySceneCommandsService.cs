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
        readonly StorySceneCommandModelEntityHandler imeHandler;
        readonly StoryScenesService scenes;

        public StorySceneCommandsService(IHorrorDbContext context, StorySceneCommandModelEntityHandler handler, StoryScenesService stories)
        {
            this.context = context;
            imeHandler = handler;
            this.scenes = stories;
        }

        public async Task<ReadStorySceneCommandModel?> TryGetAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await FindStorySceneCommandAsync(id, includeAll, token);
            return entity == default ? default : imeHandler.CreateReadModel(entity);
        }

        public Task<List<ReadStorySceneCommandModel>> GetAllAsync(bool includeAll, CancellationToken token)
        {
            return GetQuery(includeAll).Select(x => imeHandler.CreateReadModel(x)).ToListAsync(token);
        }

        public async Task<ReadStorySceneCommandModel> CreateStorySceneCommandAsync(long storySceneId, CreateStorySceneCommandModel model, bool basicValidated, CancellationToken token)
        {
            imeHandler.Validate(model, basicValidated);
            //todo: validate. add methods to mark it as validated for models
            var story = await scenes.FindStorySceneAsync(storySceneId, true, token);
            if (story == default)
                throw new HtNotFoundException($"Story with id {storySceneId} not found");

            var entity = await imeHandler.CreateEntityAsync(model, story, token);
            context.StorySceneCommands.Add(entity);
            await context.SaveChangesWrappedAsync(token);

            return imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStorySceneCommandModel> UpdateStorySceneCommandAsync(long id, UpdateStorySceneCommandModel model, bool basicValidated, CancellationToken token)
        {
            imeHandler.Validate(model, basicValidated);

            var entity = await FindStorySceneCommandAsync(id, true, token);
            if (entity == default)
                throw new HtNotFoundException($"StorySceneCommand with Id {id} not found");

            await imeHandler.UpdateEntityAsync(model, entity, token);

            await context.SaveChangesWrappedAsync(token);

            return imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteStorySceneCommandAsync(long id, CancellationToken token)
        {
            var entity = await FindStorySceneCommandAsync(id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"StorySceneCommand with Id {id} not found");

            context.StorySceneCommands.Remove(entity);
            await context.SaveChangesWrappedAsync(token);
        }

        async Task<StorySceneCommandEntity?> FindStorySceneCommandAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        IQueryable<StorySceneCommandEntity> GetQuery(bool includeAll = true)
        {
            IQueryable<StorySceneCommandEntity> query = context.StorySceneCommands;

            if (includeAll)
            {
                // TODO: this should be organized (code)
                query = query
                        .Include(x => x.Audios)
                                .ThenInclude(x => x.File)
                        .Include(x => x.Images)
                            .ThenInclude(x => x.File);
                    ;
            }
            return query;
        }
    }
}
