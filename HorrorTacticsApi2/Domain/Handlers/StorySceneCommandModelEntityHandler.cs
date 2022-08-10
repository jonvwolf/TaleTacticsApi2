using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Minigames;
using HorrorTacticsApi2.Domain.Models.Stories;

namespace HorrorTacticsApi2.Domain.Handlers
{
    public class StorySceneCommandModelEntityHandler : ModelEntityHandler
    {
        const string Separator = "֎";

        readonly ImagesService images;
        readonly AudiosService audios;
        readonly ImageModelEntityHandler imageHandler;
        readonly AudioModelEntityHandler audioHandler;

        public StorySceneCommandModelEntityHandler(ImagesService images, AudiosService audios,
            ImageModelEntityHandler imageHandler, AudioModelEntityHandler audioHandler, IHttpContextAccessor context) : base(context)
        {
            this.images = images;
            this.audios = audios;
            this.imageHandler = imageHandler;
            this.audioHandler = audioHandler;
        }
        public void Validate(UpdateStorySceneCommandModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");
        }

        public void Validate(CreateStorySceneCommandModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");
        }

        public async Task<StorySceneCommandEntity> CreateEntityAsync(CreateStorySceneCommandModel model, StorySceneEntity parent, CancellationToken token)
        {
            return new StorySceneCommandEntity(
                parent,
                model.Title,
                model.Texts,
                CreateTimersFromList(model.Timers),
                await FindImagesFromIdsAsync(model.Images, token),
                await FindAudiosFromIdsAsync(model.Audios, token),
                model.Minigames ?? new List<long>()
            );
        }

        public async Task UpdateEntityAsync(UpdateStorySceneCommandModel model, StorySceneCommandEntity entity, CancellationToken token)
        {
            entity.Title = model.Title;

            entity.Texts = model.Texts;

            if (model.Timers != default)
                entity.Timers = CreateTimersFromList(model.Timers);

            if (model.Images != default)
            {
                // TODO: improve this (performance)
                entity.Images.Clear();
                if (model.Images.Count > 0)
                    entity.Images.AddRange(await FindImagesFromIdsAsync(model.Images, token));
            }

            if (model.Audios != default)
            {
                // TODO: improve this (performance)
                entity.Audios.Clear();
                if (model.Audios.Count > 0)
                    entity.Audios.AddRange(await FindAudiosFromIdsAsync(model.Audios, token));
            }

            if(model.Minigames != default)
            {
                entity.Minigames = 0;
                if (model.Minigames.Count > 0)
                    entity.Minigames = 1;
            }
        }

        public ReadStorySceneCommandModel CreateReadModel(StorySceneCommandEntity entity)
        {
            var images = entity.Images.Select(x => imageHandler.CreateReadModel(x)).ToList();
            var audios = entity.Audios.Select(x => audioHandler.CreateReadModel(x)).ToList();

            var timers = CreateTimersFromString(entity.Timers);
            
            // TODo: change this
            var minigames = new List<ReadMinigameModel>();
            if (entity.Minigames > 0)
                minigames.Add(new ReadMinigameModel(1, "find_in_image"));

            // TODO: should I do `ToList`? inside Models?
            return new ReadStorySceneCommandModel(entity.Id, entity.Title, entity.Texts, timers, images, audios, minigames);
        }

        public List<ReadStorySceneCommandModel> CreateReadModel(List<StorySceneCommandEntity> entities)
        {
            var list = new List<ReadStorySceneCommandModel>(entities.Count);
            foreach (var item in entities)
            {
                list.Add(CreateReadModel(item));
            }
            return list;
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
