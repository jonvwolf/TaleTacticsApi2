using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.Security.Policy;
using System.Text;

namespace HorrorTacticsApi2.Services
{
    public record HubRequestModel(
        DateTimeOffset Date,
        string HubMethod,
        long? UserId,
        string GameCode
    )
    {
        public void Append(StringBuilder sb)
        {
            sb.Append(Date.ToString()).Append('\t')
                .Append(HubMethod).Append('\t')
                .Append(UserId.HasValue ? UserId : "<null>").Append('\t')
                .Append(GameCode).AppendLine();
        }
    }
}
