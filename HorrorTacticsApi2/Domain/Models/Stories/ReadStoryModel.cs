namespace HorrorTacticsApi2.Domain.Models.Stories
{
    public record ReadStoryModel(
        long Id,
        string Title,
        string Description,
        IReadOnlyList<ReadStorySceneModel> StoryScenes
    );
}
