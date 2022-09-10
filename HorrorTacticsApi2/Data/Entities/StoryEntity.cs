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

        [StringLength(ValidationConstants.Story_Title_MaxStringLength, MinimumLength = ValidationConstants.Story_Title_MinStringLength), Required]
        public string Title { get; set; } = string.Empty;

        [StringLength(ValidationConstants.Story_Description_MaxStringLength), Required]
        public string Description { get; set; } = string.Empty;

        public List<StorySceneEntity> Scenes { get; protected set; } = new List<StorySceneEntity>();

        [Required]
        public UserEntity Owner { get; set; } = UserEntity.EmptyUser;

        public StoryEntity()
        {

        }

        public StoryEntity(string title, string desc, IReadOnlyList<StorySceneEntity> scenes, UserEntity owner)
        {
            Title = title;
            Description = desc;
            Scenes = scenes.ToList();
            Owner = owner;
        }
    }
}
