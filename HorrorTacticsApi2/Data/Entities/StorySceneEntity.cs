using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class StorySceneEntity
    {
        public static readonly StorySceneEntity EmptyStoryScene = new();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public StoryEntity ParentStory { get; set; } = StoryEntity.EmptyStory;

        [StringLength(ValidationConstants.StoryScene_Title_MaxStringLength, MinimumLength = ValidationConstants.StoryScene_Title_MinStringLength), Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public List<StorySceneCommandEntity> Commands { get; set; } = new();

        public StorySceneEntity()
        {

        }

        public StorySceneEntity(StoryEntity parent,
            IReadOnlyList<StorySceneCommandEntity> commands, string title)
        {
            Title = title;
            Commands = commands.ToList();
            ParentStory = parent;
        }
    }
}
