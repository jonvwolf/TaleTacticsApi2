using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record AudioCommandModel(
        [Range(1, long.MaxValue)] long AudioId,
        bool Stop);
}
