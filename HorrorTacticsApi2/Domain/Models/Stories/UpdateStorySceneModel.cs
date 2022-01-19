using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record UpdateStorySceneModel(
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