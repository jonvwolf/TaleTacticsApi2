using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Minigames;

namespace HorrorTacticsApi2.Domain.Models.Games
{
    public record ReadGameConfiguration(
        IReadOnlyList<ReadImageModel> Images,
        IReadOnlyList<ReadAudioModel> Audios,
        IReadOnlyList<ReadMinigameModel> Minigames
    );
}
