using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NuGet.Common;

namespace HorrorTacticsApi2.Services
{
    public class MetricsBackgroundService : BackgroundService
    {
        readonly MetricsService metricsService;
        public MetricsBackgroundService(MetricsService metricsService)
        {
            this.metricsService = metricsService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return metricsService.ExecuteAsync(stoppingToken);
        }
    }
}
