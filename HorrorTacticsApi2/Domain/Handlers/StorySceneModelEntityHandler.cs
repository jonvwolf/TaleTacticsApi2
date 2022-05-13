using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Handlers;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Minigames;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Domain
{
    /// <summary>
    /// The only experts that knows how to validate and create entities and models
    /// </summary>
    public class StorySceneModelEntityHandler : ModelEntityHandler
    {
        readonly StorySceneCommandModelEntityHandler handler;
        public StorySceneModelEntityHandler(StorySceneCommandModelEntityHandler handler, IHttpContextAccessor context) : base(context)
        {
            this.handler = handler;
        }
        
        public StorySceneEntity CreateEntity(CreateStorySceneModel model, StoryEntity parent)
        {
            return new StorySceneEntity(
                parent,
                new List<StorySceneCommandEntity>(),
                model.Title
            );
        }

        public void UpdateEntity(UpdateStorySceneModel model, StorySceneEntity entity)
        {
            entity.Title = model.Title;
        }

        public ReadStorySceneModel CreateReadModel(StorySceneEntity entity)
        {
            var list = entity.Commands.Select(x => handler.CreateReadModel(x));
            return new ReadStorySceneModel(entity.Id, entity.Title, list.ToList());
        }

    }
}
