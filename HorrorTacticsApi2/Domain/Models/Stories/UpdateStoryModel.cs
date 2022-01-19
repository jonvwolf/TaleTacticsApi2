using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record UpdateStoryModel(
        [MaxLength(ValidationConstants.Story_Title_MaxStringLength),
        MinLength(ValidationConstants.Story_Title_MinStringLength),
        Required]
        string Title,
        [MaxLength(ValidationConstants.Story_Description_MaxStringLength),
        Required]
        string Description);
}
