using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record UpdateStorySceneModel(
        [MaxLength(ValidationConstants.StoryScene_Title_MaxStringLength),
        MinLength(ValidationConstants.StoryScene_Title_MinStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Title
    );
}
