using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Audio
{
    public record UpdateAudioModel(
        [MaxLength(ValidationConstants.File_Name_MaxStringLength),
        MinLength(ValidationConstants.File_Name_MinStringLength),
        Required] string Name);
}
