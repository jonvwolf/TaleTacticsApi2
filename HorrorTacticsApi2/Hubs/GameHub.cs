using HorrorTacticsApi2.Game;
using HorrorTacticsApi2.Hubs.Models;
using Jonwolfdev.Utils6.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HorrorTacticsApi2.Hubs
{
    /// <summary>
    /// TODO: check JSON security options (newtonsoft) example: can't bind to a different type
    /// TODO: validation exceptions should be converted to HubException
    /// </summary>
    public class GameHub : Hub<IGameClient>
    {
        readonly GameSaver games;
        readonly IObjectValidator<HubObjectValidator> validator;

        public GameHub(GameSaver saver, IObjectValidator<HubObjectValidator> validator)
        {
            games = saver;
            this.validator = validator;
        }

        [Authorize]
        public Task JoinGameAsHm(GameCodeModel gameCode)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            
            // TODO: validate gameCode belongs to the HM user (JWT token)
            EnsureGameCodeExists(gameCode);
            return Groups.AddToGroupAsync(Context.ConnectionId, ConstructGroupNameForHm(gameCode.GameCode));
        }

        public Task JoinGameAsPlayer(GameCodeModel gameCode)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            
            EnsureGameCodeExists(gameCode);

            return Groups.AddToGroupAsync(Context.ConnectionId, ConstructGroupNameForPlayer(gameCode.GameCode));
        }

        [Authorize]
        public Task HmSendCommand(GameCodeModel gameCode, HmCommandModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(HmCommandModel));

            EnsureGameCodeExists(gameCode);
            Clients.Group(ConstructGroupNameForHm(gameCode.GameCode)).HmReceiveLog(new TextLogModel("Command received", "Hub"));
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveHmCommand(model);
        }

        public Task PlayerSendBackHmCommand(GameCodeModel gameCode, HmCommandModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(HmCommandModel));

            EnsureGameCodeExists(gameCode);
            return Clients.Group(ConstructGroupNameForHm(gameCode.GameCode)).HmReceiveBackHmCommand(model);
        }

        public Task PlayerSendLog(GameCodeModel gameCode, TextLogModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(TextLogModel));

            EnsureGameCodeExists(gameCode);
            return Clients.Group(ConstructGroupNameForHm(gameCode.GameCode)).HmReceiveLog(model);
        }

        void EnsureGameCodeExists(GameCodeModel gameCode)
        {
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
