using HorrorTacticsApi2.Hubs.Models;

namespace HorrorTacticsApi2.Hubs
{
    public interface IGameClient
    {
        Task HmReceiveLog(TextLogModel model);
        Task HmReceiveBackHmCommand(HmCommandModel model);
        Task PlayerReceiveHmCommand(HmCommandModel model);
    }
}
