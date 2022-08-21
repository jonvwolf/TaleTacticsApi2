using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record HmCommandPredefinedModel(
        bool? ClearScreen,
        bool? StopSoundEffects,
        bool? StopBgm
    );
}
