using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Domain.Models.Minigames;
using HorrorTacticsApi2.Game;

namespace HorrorTacticsApi2.Domain
{
    public class GameModelStateHandler
    {
        public ReadGameConfiguration CreateReadModel(GameState state)
        {
            var images = new List<ReadImageModel>();
            var audios = new List<ReadAudioModel>();
            var minigames = new List<ReadMinigameModel>();

            for (int i = 0; i < state.Story.StoryScenes.Count; i++)
            {
                // Remove duplicates while adding them to the list
                images.AddRange(state.Story.StoryScenes[i].Images);
                audios.AddRange(state.Story.StoryScenes[i].Audios);
                minigames.AddRange(state.Story.StoryScenes[i].Minigames);
            }

            // TODO: performance?
            images = images.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            audios = audios.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            minigames = minigames.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            return new ReadGameConfiguration(images, audios, minigames);
        }
    }
}
