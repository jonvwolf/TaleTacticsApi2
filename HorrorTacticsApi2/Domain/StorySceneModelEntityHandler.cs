using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
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
        const string Separator = "֎";

        readonly ImagesService images;
        readonly AudiosService audios;
        readonly ImageModelEntityHandler imageHandler;
        readonly AudioModelEntityHandler audioHandler;

        public StorySceneModelEntityHandler(ImagesService images, AudiosService audios, 
            ImageModelEntityHandler imageHandler, AudioModelEntityHandler audioHandler)
        {
            this.images = images;
            this.audios = audios;
            this.imageHandler = imageHandler;
            this.audioHandler = audioHandler;
        }
        public void Validate(UpdateStorySceneModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            ValidateTexts(model.Texts);
        }

        public void Validate(CreateStorySceneModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            ValidateTexts(model.Texts);
        }

        static void ValidateTexts(IReadOnlyList<string>? texts)
        {
            if (texts != default)
            {
                foreach (var text in texts)
                {
                    if (text == default)
                        throw new HtBadRequestException($"One of the texts is null");

                    if (text.Length > ValidationConstants.StoryScene_Text_MaxStringLength)
                        throw new HtBadRequestException($"One of the texts length is greater than the allowed. Limit: {ValidationConstants.StoryScene_Text_MaxStringLength}");
                }
            }
        }

        public async Task<StorySceneEntity> CreateEntityAsync(CreateStorySceneModel model, StoryEntity parent, CancellationToken token)
        {
            return new StorySceneEntity(
                parent,
                CreateTextsFromList(model.Texts),
                CreateTimersFromList(model.Timers),
                await FindImagesFromIdsAsync(model.Images, token),
                await FindAudiosFromIdsAsync(model.Audios, token),
                model.Minigames ?? new List<long>()
            );
        }

        public async Task UpdateEntityAsync(UpdateStorySceneModel model, StorySceneEntity entity, CancellationToken token)
        {
            if (model.Texts != default && model.Texts.Count > 0)
                entity.Texts = CreateTextsFromList(model.Texts);

            if (model.Timers != default && model.Timers.Count > 0)
                entity.Timers = CreateTimersFromList(model.Timers);

            if (model.Images != default && model.Images.Count > 0)
            {
                // TODO: improve this (performance)
                entity.Images.Clear();
                entity.Images.AddRange(await FindImagesFromIdsAsync(model.Images, token));
            }

            if (model.Audios != default && model.Audios.Count > 0)
            {
                // TODO: improve this (performance)
                entity.Audios.Clear();
                entity.Audios.AddRange(await FindAudiosFromIdsAsync(model.Audios, token));
            }
        }

        public ReadStorySceneModel CreateReadModel(StorySceneEntity entity)
        {
            var images = entity.Images.Select(x => imageHandler.CreateReadModel(x)).ToList();
            var audios = entity.Audios.Select(x => audioHandler.CreateReadModel(x)).ToList();

            var timers = CreateTimersFromString(entity.Timers);
            var texts = CreateTextsFromString(entity.Texts);

            // TODo: change this
            var minigames = new List<ReadMinigameModel>();
            if (entity.Minigames > 0)
                minigames.Add(new ReadMinigameModel(1, "find_in_image"));

            // TODO: should I do `ToList`? inside Models?
            return new ReadStorySceneModel(entity.Id, texts, timers, images, audios, minigames);
        }

        static string CreateTextsFromList(IReadOnlyList<string>? textList)
        {
            string texts = string.Empty;
            if (textList != default && textList.Count > 0)
            {
                texts = string.Join(Separator, textList);
            }
            return texts;
        }

        static string CreateTimersFromList(IReadOnlyList<uint>? timerList)
        {
            string timers = string.Empty;
            if (timerList != default && timerList.Count > 0)
            {
                timers = string.Join(Separator, timerList);
            }
            return timers;
        }

        static List<string> CreateTextsFromString(string texts)
        {
            return texts.Split(Separator, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        static List<uint> CreateTimersFromString(string timers)
        {
            return timers.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Select(x => uint.Parse(x)).ToList();
        }

        async Task<List<ImageEntity>> FindImagesFromIdsAsync(IReadOnlyList<long>? imageIds, CancellationToken token)
        {
            var imagesEntities = new List<ImageEntity>();
            // TODO: optimize, only need to know the Ids exist
            if (imageIds != default && imageIds.Count > 0)
            {
                foreach (var imageId in imageIds)
                {
                    // TODO: includeAll should be here too for the TryFind
                    var entity = await images.TryFindImageAsync(imageId, token);
                    if (entity != default)
                        imagesEntities.Add(entity);
                }
            }
            return imagesEntities;
        }

        async Task<List<AudioEntity>> FindAudiosFromIdsAsync(IReadOnlyList<long>? audioIds, CancellationToken token)
        {
            var audioEntities = new List<AudioEntity>();
            // TODO: optimize, only need to know the Ids exist
            if (audioIds != default && audioIds.Count > 0)
            {
                foreach (var audioId in audioIds)
                {
                    // TODO: includeAll should be here too for the TryFind
                    var entity = await audios.TryFindAudioAsync(audioId, token);
                    if (entity != default)
                        audioEntities.Add(entity);
                }
            }
            return audioEntities;
        }
    }
}
