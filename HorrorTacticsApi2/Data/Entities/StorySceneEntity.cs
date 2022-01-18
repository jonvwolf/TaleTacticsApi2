using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class StorySceneEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public StoryEntity ParentStory { get; set; } = StoryEntity.EmptyStory;

        [MaxLength(15000)]
        public string Texts { get; protected set; } = string.Empty;
        [MaxLength(1000)]
        public string Timers { get; protected set; } = string.Empty;

        public IList<ImageEntity>? Images { get; protected set; }
        public IList<AudioEntity>? Audios { get; protected set; }

        public StorySceneEntity()
        {

        }

        public StorySceneEntity(StoryEntity parent,
            string texts, string timers, 
            IReadOnlyList<ImageEntity> images, IReadOnlyList<AudioEntity> audios)
        {
            Texts = texts;
            Timers = timers;
            Images = images.ToList();
            Audios = audios.ToList();
            ParentStory = parent;
        }
    }
}
