using HorrorTacticsApi2.Game;
using HorrorTacticsApi2.Hubs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HorrorTacticsApi2.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        readonly GameSaver games;
        public GameHub(GameSaver saver)
        {
            games = saver;
        }

        [Authorize]
        public Task JoinGameAsHm(GameCodeModel gameCode)
        {
            // TODO: validate model gameCode
            // TODO: validate gameCode belongs to the HM user (JWT token)
            EnsureGameCodeExists(gameCode);
            return Groups.AddToGroupAsync(Context.ConnectionId, ConstructGroupNameForHm(gameCode.GameCode));
        }

        public Task JoinGameAsPlayer(GameCodeModel gameCode)
        {
            // TODO: validate model gameCode
            EnsureGameCodeExists(gameCode);
            return Groups.AddToGroupAsync(Context.ConnectionId, ConstructGroupNameForPlayer(gameCode.GameCode));
        }

        [Authorize]
        public Task HmSendImageCommand(GameCodeModel gameCode, ImageCommandModel model)
        {
            EnsureGameCodeExists(gameCode);
            // TODO: validate model
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveImageCommand(model);
        }

        [Authorize]
        public Task HmSendAudioCommand(GameCodeModel gameCode, AudioCommandModel model)
        {
            EnsureGameCodeExists(gameCode);
            // TODO: validate model
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveAudioCommand(model);
        }

        [Authorize]
        public Task HmSendTextCommand(GameCodeModel gameCode, TextCommandModel model)
        {
            EnsureGameCodeExists(gameCode);
            // TODO: validate model
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveTextCommand(model);
        }

        [Authorize]
        public Task HmSendMinigameCommand(GameCodeModel gameCode, MinigameCommandModel model)
        {
            EnsureGameCodeExists(gameCode);
            // TODO: validate model
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveMinigameCommand(model);
        }

        public Task PlayerSendLog(GameCodeModel gameCode, PlayerTextLogModel model)
        {
            // TODO: validate model
            EnsureGameCodeExists(gameCode);
            return Clients.Group(ConstructGroupNameForHm(gameCode.GameCode)).HmReceiveLog(model);
        }

        void EnsureGameCodeExists(GameCodeModel gameCode)
        {
            // TODO: make sure HubException does not give stack trace or something
            if (!games.DoesGameCodeExist(gameCode.GameCode))
                throw new HubException("Game code does not exist");
        }

        static string ConstructGroupNameForHm(string gameCode)
        {
            return "HM-" + gameCode;
        }

        static string ConstructGroupNameForPlayer(string gameCode)
        {
            return "PL-" + gameCode;
        }
    }
}
