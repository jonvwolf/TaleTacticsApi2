using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Game;

namespace HorrorTacticsApi2.Domain
{
    public class GamesService
    {
        readonly StoriesService stories;
        readonly GameSaver gameSaver;
        readonly GameModelStateHandler gameModelStateHandler;
        public GamesService(StoriesService stories, GameSaver gameSaver, GameModelStateHandler gameModelStateHandler)
        {
            this.stories = stories;
            this.gameSaver = gameSaver;
            this.gameModelStateHandler = gameModelStateHandler;
        }

        public async Task<ReadGameCreatedModel> CreateGameAsync(long storyId, CancellationToken token)
        {
            // TODO: performance, only need to check if it exists
            var story = await stories.TryGetAsync(storyId, token) ?? throw new HtNotFoundException($"Story id not found: {storyId}");

            // TODO: remember that services do not handle records/models. Change this
            return new ReadGameCreatedModel(gameSaver.CreateGame(story));
        }

        public ReadGameConfiguration GetGameConfiguration(string gameCode)
        {
            var state = gameSaver.TryGetGameState(gameCode);
            if (state == default)
                throw new HtNotFoundException("Game code not found");

            return gameModelStateHandler.CreateReadModel(state);
        }
    }
}
