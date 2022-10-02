using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class StorySceneCommandEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public StorySceneEntity ParentStoryScene { get; set; } = StorySceneEntity.EmptyStoryScene;

        [StringLength(ValidationConstants.StorySceneCommand_Title_MaxStringLength, 
            MinimumLength = ValidationConstants.StorySceneCommand_Title_MinStringLength), Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Validation length is specific (no need to share)
        /// </summary>
        [MaxLength(ValidationConstants.StoryScene_Text_MaxStringLength)]
        public string? Texts { get; set; } = default;
        /// <summary>
        /// Validation length is specific (no need to share)
        /// </summary>
        [MaxLength(1000)]
        public string Timers { get; set; } = string.Empty;

        public List<ImageEntity> Images { get; protected set; } = new();
        public List<AudioEntity> Audios { get; protected set; } = new();

        public long Minigames { get; set; }

        public string? Comments { get; set; }

        public bool StartInternalTimer { get; set; }
        public StorySceneCommandEntity()
        {

        }

        public StorySceneCommandEntity(StorySceneEntity parent, string title, string? texts, string timers, 
            IReadOnlyList<ImageEntity> images, IReadOnlyList<AudioEntity> audios, IReadOnlyList<long> minigames,
            string? comments, bool startInternalTimer)
        {
            ParentStoryScene = parent;
            Title = title;
            Texts = texts;
            Timers = timers;
            // TODO: be consistent, in some other models/entities are not doing this...
            // -> It should be AddRange, right?
            Images = images.ToList();
            Audios = audios.ToList();
            // TODO: change this
            Minigames = minigames.Count > 0 ? minigames[0] : 0;
            Comments = comments;
            StartInternalTimer = startInternalTimer;
        }
    }
}
