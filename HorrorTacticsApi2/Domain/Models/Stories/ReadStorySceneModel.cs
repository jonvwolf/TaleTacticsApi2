namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record ReadStorySceneModel(
        long Id,
        string Title,
        IReadOnlyList<ReadStorySceneCommandModel> StorySceneCommands
    );
}
