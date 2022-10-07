using HorrorTacticsApi2.Domain.Models.Stories;

namespace HorrorTacticsApi2.Domain.Models.Games
{
    public record ReadGameStateModel(
        string Code,
        string Notes,
        ReadStoryModel Story
    );
}
