namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record UpdateStorySceneModel(
        IReadOnlyList<string>? Texts,
        IReadOnlyList<long>? Minigames,
        IReadOnlyList<uint>? Timers,
        IReadOnlyList<long>? Images,
        IReadOnlyList<long>? Audios
    );
}