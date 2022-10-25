using System.Text;

namespace HorrorTacticsApi2.Services
{
    public record RequestModel(
        DateTimeOffset Date,
        string Url,
        string Method,
        long? UserId,
        int StatusCode
    )
    {
        public void Append(StringBuilder sb)
        {
            sb.Append(Date.ToString()).Append('\t')
                .Append(Url).Append('\t')
                .Append(Method).Append('\t')
                .Append(UserId.HasValue ? UserId : "<null>").Append('\t')
                .Append(StatusCode).AppendLine();
        }
    }
}
