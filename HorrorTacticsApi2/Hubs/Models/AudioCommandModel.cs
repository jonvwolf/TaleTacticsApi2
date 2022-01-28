using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record AudioCommandModel(
        [property: Range(1, long.MaxValue)] long AudioId,
        bool Stop);
}
