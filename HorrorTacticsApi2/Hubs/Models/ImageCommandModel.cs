using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record ImageCommandModel(
        [Range(1, long.MaxValue)] long ImageId);
}
