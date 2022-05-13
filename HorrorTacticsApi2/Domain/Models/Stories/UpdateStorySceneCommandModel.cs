using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record UpdateStorySceneCommandModel(
        [MaxLength(ValidationConstants.StorySceneCommand_Title_MaxStringLength),
        MinLength(ValidationConstants.StorySceneCommand_Title_MinStringLength),
        Required]
        string Title,

        [MaxLength(ValidationConstants.StoryScene_Items_List_MaxLength)]
        IReadOnlyList<string>? Texts,

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