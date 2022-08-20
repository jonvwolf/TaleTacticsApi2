using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record HmCommandModel(
        [property: MaxLength(20)] List<long>? AudioIds,
        [property: Range(1, long.MaxValue)] long? ImageId,
        [property: Range(1, long.MaxValue)] long? MinigameId,
        [property: MinLength(ValidationConstants.StoryScene_Text_MinStringLength), MaxLength(ValidationConstants.StoryScene_Text_MaxStringLength), 
        RegularExpression(ValidationConstants.RegularExpressionForAllStrings)] string? Text
    );
}
