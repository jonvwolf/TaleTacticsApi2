using HorrorTacticsApi2.Domain.Exceptions;
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

        public string CreateGame(ReadStoryModel story)
        {
            for(int i = 0; i < MaxTriesGameCode; i++)
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

                    games[code] = new GameState(story);
                }
                return code;
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

        public bool DoesGameCodeExist(string gameCode)
        {
            lock (lockObj)
            {
                return games.ContainsKey(gameCode);
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
