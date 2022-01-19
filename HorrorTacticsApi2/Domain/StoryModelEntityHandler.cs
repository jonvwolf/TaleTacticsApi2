using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Domain
{
    /// <summary>
    /// The only experts that knows how to validate and create entities and models
    /// </summary>
    public class StoryModelEntityHandler : ModelEntityHandler
    {
        public StoryModelEntityHandler()
        {
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

        public StoryEntity CreateEntity(CreateStoryModel model)
        {
            return new StoryEntity();
        }

        public void UpdateEntity(UpdateStoryModel model, StoryEntity entity)
        {
            //entity.File.Name = model.Name;
        }

        public ReadStoryModel CreateReadModel(StoryEntity entity)
        {
            return new ReadStoryModel(entity.Id);
        }



    }
}
