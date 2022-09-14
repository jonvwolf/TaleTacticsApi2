using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Game;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Domain
{
    public class GamesService
    {
        readonly StoriesService stories;
        readonly GameSaver gameSaver;
        readonly GameModelStateHandler gameModelStateHandler;
        readonly int MaxGamesPerUser = 10;

        public GamesService(StoriesService stories, GameSaver gameSaver, GameModelStateHandler gameModelStateHandler, IOptions<AppSettings> settings)
        {
            this.stories = stories;
            this.gameSaver = gameSaver;
            this.gameModelStateHandler = gameModelStateHandler;
            MaxGamesPerUser = settings.Value.MaxGamesPerUser;
        }

        public async Task<ReadGameCreatedModel> CreateGameAsync(UserJwt user, long storyId, CancellationToken token)
        {
            // TODO: performance, only need to check if it exists
            var story = await stories.TryGetAsync(user, storyId, token) ?? throw new HtNotFoundException($"Story id not found: {storyId}");

            if (gameSaver.GetTotalGames() >= MaxGamesPerUser)
                throw new HtConflictException("Cannot create more games. Delete one first and try again");

            // TODO: remember that services do not handle records/models. Change this
            return new ReadGameCreatedModel(gameSaver.CreateGame(story, user.Id));
        }

        public void DeleteGame(UserJwt user, string gameCode)
        {
            gameSaver.RemoveGame(gameCode, user.Id);
        }

        public IReadOnlyList<ReadGameStateModel> GetAllGames(UserJwt user)
        {
            return gameSaver.GetAllGames(user.Id);
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
