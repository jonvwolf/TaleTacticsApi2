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
        public Task HmSendImageCommand(GameCodeModel gameCode, ImageCommandModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(ImageCommandModel));

            EnsureGameCodeExists(gameCode);
            
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveImageCommand(model);
        }

        [Authorize]
        public Task HmSendAudioCommand(GameCodeModel gameCode, AudioCommandModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(AudioCommandModel));

            EnsureGameCodeExists(gameCode);
            
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveAudioCommand(model);
        }

        [Authorize]
        public Task HmSendTextCommand(GameCodeModel gameCode, TextCommandModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(TextCommandModel));

            EnsureGameCodeExists(gameCode);
            
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveTextCommand(model);
        }

        [Authorize]
        public Task HmSendMinigameCommand(GameCodeModel gameCode, MinigameCommandModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(MinigameCommandModel));

            EnsureGameCodeExists(gameCode);
            
            return Clients.Group(ConstructGroupNameForPlayer(gameCode.GameCode)).PlayerReceiveMinigameCommand(model);
        }

        public Task PlayerSendLog(GameCodeModel gameCode, PlayerTextLogModel model)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));
            validator.Validate(model, nameof(PlayerTextLogModel));

            EnsureGameCodeExists(gameCode);
            return Clients.Group(ConstructGroupNameForHm(gameCode.GameCode)).HmReceiveLog(model);
        }

        void EnsureGameCodeExists(GameCodeModel gameCode)
        {
            validator.Validate(gameCode, nameof(GameCodeModel));

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
