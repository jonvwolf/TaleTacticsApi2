using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.Extensions.Options;
using System.Text;

namespace HorrorTacticsApi2.Game
{
    /// <summary>
    /// Multi thread safe
    /// </summary>
    public class GameSaver
    {
        readonly Dictionary<string, GameState> games = new();
        
        readonly Random random = new();

        const int MaxTriesGameCode = 100;
        const int MaxTriesBeforeAdding = 10;

        readonly object lockObj = new();

        readonly int maxGameCodeLength;
        int currentGameCodeLength;

        public GameSaver(IOptions<AppSettings> settings)
        {
            currentGameCodeLength = settings.Value.GameCodeInitialLength;
            maxGameCodeLength = settings.Value.GameCodeMaxLength;
        }

        public string CreateGame(ReadStoryModel story, long userId)
        {
            // Start at 1 because 0 % 10 will increase the game code length
            for(int i = 1; i <= MaxTriesGameCode; i++)
            {
                // If enough tries have passed, increase the game code length
                if (i % MaxTriesBeforeAdding == 0)
                {
                    currentGameCodeLength++;
                    if (currentGameCodeLength > maxGameCodeLength)
                        currentGameCodeLength = maxGameCodeLength;
                }

                var code = GenerateGameCode(currentGameCodeLength, random);

                lock (lockObj)
                {
                    if (games.ContainsKey(code))
                        continue;

                    games[code] = new GameState(code, story, userId);
                    return code;
                }
            }

            throw new HtServiceUnavailableException("Couldn't generate a game code... Try again");
        }

        public GameState? TryGetGameState(string gameCode)
        {
            lock (lockObj)
            {
                games.TryGetValue(gameCode, out var gameState);
                return gameState;
            }
        }

        public bool DoesGameCodeExist(string gameCode, long? userId)
        {
            lock (lockObj)
            {
                if (userId.HasValue)
                {
                    if (games.TryGetValue(gameCode, out var state))
                    {
                        if (state.OwnerId == userId)
                            return true;
                    }
                    return false;
                }
                else
                {
                    return games.ContainsKey(gameCode);
                }
            }
        }

        public void RemoveGame(string gameCode, long userId)
        {
            lock (lockObj)
            {
                if (games.TryGetValue(gameCode, out var state))
                {
                    if (state.OwnerId == userId)
                        games.Remove(gameCode);
                }
            }
        }

        public int GetTotalGames()
        {
            lock (lockObj)
            {
                return games.Count;
            }
        }

        public IReadOnlyList<ReadGameStateModel> GetAllGames(long userId)
        {
            lock (lockObj)
            {
                // TODO: only model handlers can do this (create models)
                return games.Where(x => x.Value.OwnerId == userId).Select(pair => new ReadGameStateModel(pair.Key, pair.Value.Story)).ToList();
            }
        }

        static string GenerateGameCode(int length, Random random)
        {
            var sb = new StringBuilder(length);
            string guid = Guid.NewGuid().ToString();
            string[] parts = guid.Split("-");

            if (parts.Length == 0)
                throw new InvalidOperationException("Invalid guid");

            int currentPartsIndex = 0;
            for (int i = 0; i < length; i++)
            {
                // from guid xxxx-xxx-xxx-xxxx
                // parts[n] = xxxx
                // random will get a random character from parts[n] (from 0 to parts[n].Length)
                char letter = parts[currentPartsIndex][random.Next(0, parts[currentPartsIndex].Length)];
                sb.Append(letter);

                // If length is bigger than parts.Length, it will just loop back to the beginning
                currentPartsIndex++;
                if (currentPartsIndex >= parts.Length)
                    currentPartsIndex = 0;
            }

            return sb.ToString();
        }
    }
}
