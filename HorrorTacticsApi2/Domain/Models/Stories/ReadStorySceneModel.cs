using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Audio;

namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record ReadStorySceneModel(
        long Id,
        IReadOnlyList<string> Texts,
        IReadOnlyList<uint> Timers,
        IReadOnlyList<ReadImageModel> Images,
        IReadOnlyList<ReadAudioModel> Audios
    );
}
