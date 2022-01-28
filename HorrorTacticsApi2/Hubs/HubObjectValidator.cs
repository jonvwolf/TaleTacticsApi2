using Jonwolfdev.Utils6.Validation;
using Microsoft.AspNetCore.SignalR;

namespace HorrorTacticsApi2.Hubs
{
    public class HubObjectValidator : ObjectValidator<HubObjectValidator>
    {
        public override void ThrowException(string message, string paramName)
        {
            // TODO: security. check if paramName can be manipulated
            throw new HubException($"{paramName}: {message}");
        }
    }
}
