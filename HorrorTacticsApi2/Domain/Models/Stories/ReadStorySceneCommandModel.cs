using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Minigames;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record ReadStorySceneCommandModel(
        long Id,
        string Title,
        string? Texts,
        IReadOnlyList<uint> Timers,
        IReadOnlyList<ReadImageModel> Images,
        IReadOnlyList<ReadAudioModel> Audios,
        IReadOnlyList<ReadMinigameModel> Minigames,
        string? Comments,
        bool StartInternalTimer
    );
}
