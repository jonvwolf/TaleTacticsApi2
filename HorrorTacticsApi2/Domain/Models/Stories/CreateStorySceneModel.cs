using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record CreateStorySceneModel(
        [MaxLength(ValidationConstants.StoryScene_Title_MaxStringLength),
        MinLength(ValidationConstants.StoryScene_Title_MinStringLength),
        Required]
        string Title
    );
}
