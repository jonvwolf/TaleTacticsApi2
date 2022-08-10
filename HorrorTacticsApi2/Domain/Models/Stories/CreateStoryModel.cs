using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record CreateStoryModel(
        [MaxLength(ValidationConstants.Story_Title_MaxStringLength),
        MinLength(ValidationConstants.Story_Title_MinStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Title,
        [MaxLength(ValidationConstants.Story_Description_MaxStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Description
    );
}
