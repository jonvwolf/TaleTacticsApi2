using HorrorTacticsApi2.Common;
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
    public class StorySceneModelEntityHandler : ModelEntityHandler
    {
        const string Separator = "֎";

        readonly ImagesService images;
        readonly AudioService audios;
        public StorySceneModelEntityHandler(ImagesService images, AudioService audios)
        {
            this.images = images;
            this.audios = audios;
        }
        public void Validate(UpdateStorySceneModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public void Validate(CreateStorySceneModel model, bool basicValidated)
        {
            // TODO: set a limit for the lists (validation)
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            if (model.Texts != default)
            {
                // TODO: this is repeated code in StorySceneEntity
                foreach (var text in model.Texts)
                {
                    if (text.Length > ValidationConstants.StoryScene_Text_MaxStringLength)
                        throw new HtBadRequestException($"One of the texts length is greater than the allowed. Limit: {ValidationConstants.StoryScene_Text_MaxStringLength}");

                    if (text == default)
                        throw new HtBadRequestException($"One of the texts is null");
                }
            }
            
        }

        public async Task<StorySceneEntity> CreateEntityAsync(CreateStorySceneModel model, CancellationToken token)
        {
            string texts = string.Empty;
            if (model.Texts != default && model.Texts.Count > 0)
            {
                texts = string.Join(Separator, model.Texts);
            }

            string timers = string.Empty;
            if (model.Timers != default && model.Timers.Count > 0)
            {
                timers = string.Join(Separator, model.Timers);
            }

            var imagesEntities = new List<ImageEntity>();
            // TODO: optimize, only need to know the Ids exist
            if (model.Images != default && model.Images.Count > 0)
            {
                foreach (var imageId in model.Images)
                {
                    var entity = await images.TryGetAsync(imageId, token);
                    if (entity != default)
                        imagesEntities.Add(entity);
                }
            }

            return new StorySceneEntity();
        }

        public void UpdateEntity(UpdateStorySceneModel model, StorySceneEntity entity)
        {
            
        }

        public ReadStorySceneModel CreateReadModel(StorySceneEntity entity)
        {
            return new ReadStorySceneModel();
        }



    }
}
