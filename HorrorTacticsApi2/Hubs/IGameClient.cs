using HorrorTacticsApi2.Hubs.Models;

namespace HorrorTacticsApi2.Hubs
{
    public interface IGameClient
    {
        Task HmReceiveLog(PlayerTextLogModel model);
        Task PlayerReceiveImageCommand(ImageCommandModel model);
        Task PlayerReceiveAudioCommand(AudioCommandModel model);
        Task PlayerReceiveTextCommand(TextCommandModel model);
        Task PlayerReceiveMinigameCommand(MinigameCommandModel model);
    }
}
