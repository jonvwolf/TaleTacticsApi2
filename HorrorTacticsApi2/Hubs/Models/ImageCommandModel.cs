using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record ImageCommandModel(
        [property: Range(1, long.MaxValue)] long ImageId);
}
