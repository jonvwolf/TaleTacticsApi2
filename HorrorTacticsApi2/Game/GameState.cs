using HorrorTacticsApi2.Domain.Models.Stories;

namespace HorrorTacticsApi2.Game
{
    public class GameState
    {
        public ReadStoryModel Story { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset AccessedAt { get; protected set; }

        public GameState(ReadStoryModel story)
        {
            Story = story;
            CreatedAt = DateTimeOffset.UtcNow;
            AccessedAt = DateTimeOffset.UtcNow;
        }
    }
}
