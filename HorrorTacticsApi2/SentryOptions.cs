using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2
{
    public class SentryOptions
    {
        public string Dsn { get; set; } = string.Empty;
        public bool Debug { get; set; }
        public double TracesSampleRate { get; set; } = 0.2;
        public bool Enable { get; set; } = false;
    }
}
