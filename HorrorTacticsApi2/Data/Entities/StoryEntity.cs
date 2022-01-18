using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class StoryEntity
    {
        public static readonly StoryEntity EmptyStory = new();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(ValidationConstants.Story_Title_MaxStringLength), Required]
        public string Title { get; set; } = string.Empty;

        [StringLength(ValidationConstants.Story_Description_MaxStringLength), Required]
        public string Description { get; set; } = string.Empty;

        public IList<StorySceneEntity> Scenes { get; protected set; } = new List<StorySceneEntity>();

        public StoryEntity()
        {

        }

        public StoryEntity(string title, string desc, IReadOnlyList<StorySceneEntity> scenes)
        {
            Title = title;
            Description = desc;
            Scenes = scenes.ToList();
        }
    }
}
