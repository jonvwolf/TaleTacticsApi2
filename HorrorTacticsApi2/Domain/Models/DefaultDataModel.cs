using HorrorTacticsApi2.Common;
using Jonwolfdev.Utils6.Validation;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models
{
    public record DefaultDataModel(
        [property: MaxLength(ValidationConstants.Story_Title_MaxStringLength),
        MinLength(ValidationConstants.Story_Title_MinStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string StoryTitle,

        [property: MaxLength(ValidationConstants.Story_Description_MaxStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string StoryDescription,

        [property: Required, ValidateComplexList, MinLength(1)]
        IReadOnlyDictionary<string, DefaultDataImageModel> Images,

        [property: Required, ValidateComplexList, MinLength(1)]
        IReadOnlyDictionary<string, DefaultDataSoundModel> Sounds,

        // TODO: This could be easily fixed by having a `Validate` method
        [property: Required, MinLength(1)]
        IReadOnlyDictionary<string, string> Texts,

        [property: Required, ValidateComplexList, MinLength(1)]
        IReadOnlyList<DefaultDataSceneModel> Scenes
    );

    public record DefaultDataImageModel(
        [property: Range(1, long.MaxValue)]
        long Size
    );

    public record DefaultDataSoundModel(
        [property: Range(1, long.MaxValue)]
        long Size,
        [property: Required]
        bool? IsBgm
    );

    public record DefaultDataSceneModel(
        [property: MaxLength(ValidationConstants.StoryScene_Title_MaxStringLength),
        MinLength(ValidationConstants.StoryScene_Title_MinStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Title,

        [property: Required, ValidateComplexList, MinLength(1)]
        IReadOnlyList<DefaultDataCommandModel> Commands
    );

    public record DefaultDataCommandModel(
        [MaxLength(ValidationConstants.StorySceneCommand_Title_MaxStringLength),
        MinLength(ValidationConstants.StorySceneCommand_Title_MinStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Title,

        string? ImageId,
        string? TextId,
        string? Comments,
        [Range(1, 999)]
        int? Timer,
        bool StartInternalTimer,

        IReadOnlyList<string>? SoundIds
    );
}
