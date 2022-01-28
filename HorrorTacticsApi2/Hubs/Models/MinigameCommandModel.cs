using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record MinigameCommandModel(
        [property: Range(1, long.MaxValue)] long MinigameId);
}
