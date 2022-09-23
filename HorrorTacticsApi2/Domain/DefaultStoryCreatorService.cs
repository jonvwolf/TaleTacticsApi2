using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Models;
using Jonwolfdev.Utils6.Validation;
using Newtonsoft.Json;

namespace HorrorTacticsApi2.Domain
{
    public class DefaultStoryCreatorService
    {
        readonly StoryScenesService _storyScenesService; 
        readonly StoriesService _storiesService; 
        readonly StorySceneCommandsService _storySceneCommandsService;
        readonly IHorrorDbContext _context;
        public DefaultStoryCreatorService(StoryScenesService storyScenesService, StoriesService storiesService, StorySceneCommandsService storySceneCommandsService,
            IHorrorDbContext context)
        {
            _storiesService = storiesService;
            _storyScenesService = storyScenesService;
            _storySceneCommandsService = storySceneCommandsService;
            _context = context;
        }

        public async Task Create(UserEntity entity, CancellationToken token)
        {
            var contents = await File.ReadAllTextAsync(Path.Combine(Constants.DefaultFilePath, Constants.DefaultDataFile), token);
            var data = JsonConvert.DeserializeObject<DefaultDataModel>(contents);
            if (data == default)
                throw new InvalidOperationException("Data was null/empty");

            ObjectValidator<DefaultStoryCreatorService>.ValidateObject(data, nameof(data));

            // TODO: this should go through services and modelentity handlers
            var story = new StoryEntity(data.StoryTitle, data.StoryDescription, new List<StorySceneEntity>(), entity);
            _context.Stories.Add(story);

            var images = new Dictionary<string, ImageEntity>(data.Images.Count);
            foreach (var image in data.Images)
            {
                var imageEntity = CreateImage(image.Key, image.Value, entity);
                images.Add(image.Key, imageEntity);
                _context.Images.Add(imageEntity);
            }

            var audios = new Dictionary<string, AudioEntity>(data.Sounds.Count);
            foreach (var sound in data.Sounds)
            {
                var audioEntity = CreateAudio(sound.Key, sound.Value, entity);
                audios.Add(sound.Key, audioEntity);
                _context.Audios.Add(audioEntity);
            }

            foreach (var scene in data.Scenes)
            {
                var sceneEntity = new StorySceneEntity(story, new List<StorySceneCommandEntity>(), scene.Title);
                _context.StoryScenes.Add(sceneEntity);

                foreach (var command in scene.Commands)
                {
                    
                    string? text = command.TextId != default ? data.Texts[command.TextId] : default;
                    var imageEntity = command.ImageId != default ? new List<ImageEntity>() { images[command.ImageId] } : new List<ImageEntity>();

                    List<AudioEntity> soundEntities = new();
                    if (command.SoundIds != default)
                    {
                        foreach (var soundId in command.SoundIds)
                        {
                            soundEntities.Add(audios[soundId]);
                        }
                    }
;
                    _context.StorySceneCommands.Add(new StorySceneCommandEntity(sceneEntity, command.Title, text, string.Empty, imageEntity, soundEntities, new List<long>()));
                }
            }

            // Do not pass token as we don't want to cancel.
            // Do whatever it is possible to create the default story for the user
            await _context.SaveChangesWrappedAsync(CancellationToken.None);
        }

        static ImageEntity CreateImage(string id, DefaultDataImageModel info, UserEntity user)
        {
            var fileEntity = new FileEntity(id, FileFormatEnum.JPG, id, info.Size, user)
            {
                IsDefault = true
            };

            var entity = new ImageEntity(fileEntity, 0, 0);
            return entity;
        }

        static AudioEntity CreateAudio(string id, DefaultDataSoundModel info, UserEntity user)
        {
            var fileEntity = new FileEntity(id, FileFormatEnum.MP3, id, info.Size, user)
            {
                IsDefault = true
            };

            if (!info.IsBgm.HasValue)
                throw new InvalidOperationException($"{nameof(DefaultDataSoundModel.IsBgm)} is null. Id: {id}");

            var entity = new AudioEntity(fileEntity, info.IsBgm.Value, 0);
            return entity;
        }
    }
}
