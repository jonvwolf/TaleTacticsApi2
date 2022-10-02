using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Models;
using Jonwolfdev.Utils6.Validation;
using Newtonsoft.Json;

namespace HorrorTacticsApi2.Domain
{
    public class DefaultStoryCreatorService
    {
        readonly IHorrorDbContext _context;
        const string FilenameSeparator = "_";

        public DefaultStoryCreatorService(IHorrorDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserEntity entity, CancellationToken token)
        {
            var contents = await File.ReadAllTextAsync(Path.Combine(Constants.GetDefaultFilePath(), Constants.DefaultDataFile), token);
            var data = JsonConvert.DeserializeObject<DefaultDataModel>(contents);
            if (data == default)
                throw new InvalidOperationException("Data was null/empty");

            ObjectValidator<DefaultStoryCreatorService>.ValidateObject(data, nameof(data));

            // TODO: this should go through services and modelentity handlers (careful with circular dependency...)
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
                    _context.StorySceneCommands.Add(new StorySceneCommandEntity(sceneEntity, command.Title, text, string.Empty, imageEntity, soundEntities, new List<long>(), command.Comments, command.StartInternalTimer));
                }
            }

            await _context.SaveChangesWrappedAsync(token);
        }

        static ImageEntity CreateImage(string id, DefaultDataImageModel info, UserEntity user)
        {
            var fileEntity = new FileEntity(id, FileFormatEnum.JPG, RandomizeFilename(id), info.Size, user)
            {
                IsDefault = true
            };

            var entity = new ImageEntity(fileEntity, 0, 0);
            return entity;
        }

        static AudioEntity CreateAudio(string id, DefaultDataSoundModel info, UserEntity user)
        {
            // TODO: when inserting duplicates, it would not throw duplicated key exception and empty values were in db or in memory db cache?
            // - confirm first
            var fileEntity = new FileEntity(id, FileFormatEnum.MP3, RandomizeFilename(id), info.Size, user)
            {
                IsDefault = true
            };

            if (!info.IsBgm.HasValue)
                throw new InvalidOperationException($"{nameof(DefaultDataSoundModel.IsBgm)} is null. Id: {id}");

            var entity = new AudioEntity(fileEntity, info.IsBgm.Value, 0);
            return entity;
        }

        static string RandomizeFilename(string id)
        {
            return Guid.NewGuid().ToString() + FilenameSeparator + id;
        }

        public static string GetFullPath(string filename)
        {
            var parts = filename.Split(FilenameSeparator, 2);
            return parts[1];
        }
    }
}
