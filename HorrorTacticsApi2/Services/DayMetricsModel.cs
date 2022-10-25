using System.Text;

namespace HorrorTacticsApi2.Services
{
    public record DayMetricsModel(DateTimeOffset Date)
    {
        public List<RequestModel> Requests { get; set; } = new();
        public List<HubRequestModel> HubRequests { get; set; } = new();

        public override string ToString()
        {
            if (Requests.Count == 0 && HubRequests.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(250);

            sb.Append("Date: ").AppendLine(Date.ToString());

            foreach(var request in Requests)
            {
                request.Append(sb);
            }
            foreach (var hubRequest in HubRequests)
            {
                hubRequest.Append(sb);
            }

            return sb.ToString();
        }
    }
}
