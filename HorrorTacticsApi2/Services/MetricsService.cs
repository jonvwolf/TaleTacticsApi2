using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Services
{
    public class MetricsService
    {
        DayMetricsModel _dayMetricsModel;

        readonly object _locker = new();
        readonly AppSettings _appSettings;
        public MetricsService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
            _dayMetricsModel = CreateModel();
        }

        public void AddRequest(RequestModel model)
        {
            lock (_locker)
            {
                _dayMetricsModel.Requests.Add(model);
            }
        }

        public void AddHubRequest(HubRequestModel model)
        {
            lock (_locker)
            {
                _dayMetricsModel.HubRequests.Add(model);
            }
        }

        static DayMetricsModel CreateModel()
        {
            return new DayMetricsModel(new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, DateTimeOffset.Now.Offset));
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var wait = TimeSpan.FromMinutes(_appSettings.MetricsServiceWaitForMinutes);

            if (!Directory.Exists(Constants.MetricsFolder))
                Directory.CreateDirectory(Constants.MetricsFolder);

            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                try
                {
                    await Task.Delay(wait, stoppingToken);
                    await SaveAsync(false);
                }
                catch (OperationCanceledException)
                {
                    await SaveAsync(true);
                    break;
                }
            }
        }

        async Task SaveAsync(bool force)
        {
            if (_dayMetricsModel.Date.Day != DateTimeOffset.Now.Day || force)
            {
                DayMetricsModel? model;
                lock (_locker)
                {
                    model = _dayMetricsModel;
                    _dayMetricsModel = CreateModel();
                }

                var output = model.ToString();

                if (!string.IsNullOrEmpty(output))
                    await File.WriteAllTextAsync(Path.Combine(Constants.MetricsFolder, $"metrics-{model.Date.ToUnixTimeMilliseconds()}-{force}.txt"), output, CancellationToken.None);
            }
        }
    }
}
