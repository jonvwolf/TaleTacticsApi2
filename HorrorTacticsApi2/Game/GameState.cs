using HorrorTacticsApi2.Domain.Models.Stories;

namespace HorrorTacticsApi2.Game
{
    /// <summary>
    /// Public properties are immutable
    /// </summary>
    public class GameState
    {
        public string Code { get; }
        public ReadStoryModel Story { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset AccessedAt { get; protected set; }

        public GameState(string code, ReadStoryModel story)
        {
            Code = code;
            Story = story;
            CreatedAt = DateTimeOffset.UtcNow;
            AccessedAt = DateTimeOffset.UtcNow;
        }
    }
}
