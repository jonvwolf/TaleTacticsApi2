using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record CreateStorySceneCommandModel(
        [MaxLength(ValidationConstants.StorySceneCommand_Title_MaxStringLength),
        MinLength(ValidationConstants.StorySceneCommand_Title_MinStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Title,

        [MinLength(ValidationConstants.StoryScene_Text_MinStringLength), MaxLength(ValidationConstants.StoryScene_Text_MaxStringLength),
        RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string? Texts,

        [MaxLength(ValidationConstants.StoryScene_Items_List_MaxLength)]
        IReadOnlyList<long>? Minigames,

        [MaxLength(ValidationConstants.StoryScene_Items_List_MaxLength)]
        IReadOnlyList<uint>? Timers,

        [MaxLength(ValidationConstants.StoryScene_Items_List_MaxLength)]
        IReadOnlyList<long>? Images,

        [MaxLength(ValidationConstants.StoryScene_Items_List_MaxLength)]
        IReadOnlyList<long>? Audios
    );
}
