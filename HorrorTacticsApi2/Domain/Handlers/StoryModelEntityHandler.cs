using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Handlers;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Domain
{
    /// <summary>
    /// The only experts that knows how to validate and create entities and models
    /// </summary>
    public class StoryModelEntityHandler : ModelEntityHandler
    {
        readonly StorySceneModelEntityHandler scene;
        public StoryModelEntityHandler(StorySceneModelEntityHandler scene, IHttpContextAccessor context) : base(context)
        {
            this.scene = scene;
        }

        public void Validate(UpdateStoryModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public void Validate(CreateStoryModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public StoryEntity CreateEntity(UserEntity user, CreateStoryModel model)
        {
            return new StoryEntity(model.Title, model.Description, new List<StorySceneEntity>(), user);
        }

        public void UpdateEntity(UpdateStoryModel model, StoryEntity entity)
        {
            entity.Title = model.Title;
            entity.Description = model.Description;
        }

        public ReadStoryModel CreateReadModel(StoryEntity entity)
        {
            return new ReadStoryModel(entity.Id, entity.Title, entity.Description, entity.Scenes.Select(x => scene.CreateReadModel(x)).OrderBy(x => x.Title).ToList());
        }
    }
}
