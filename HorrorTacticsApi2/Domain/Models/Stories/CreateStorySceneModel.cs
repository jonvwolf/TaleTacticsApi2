namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record CreateStorySceneModel(
        IList<string>? Texts,
        IList<long>? Minigames,
        IList<uint>? Timers,
        IList<long>? Images,
        IList<long>? Audios
    );
}
