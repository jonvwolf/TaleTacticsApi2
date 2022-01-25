using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Games
{
    public record CreateGameModel(
        [Range(1, long.MaxValue)] long StoryId);
}
