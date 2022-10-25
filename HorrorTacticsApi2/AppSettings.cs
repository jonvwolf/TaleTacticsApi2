using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2
{
    public class AppSettings
    {
        [Required, MinLength(3)]
        public string MainPassword { get; set; } = "";
        [Range(64, 49152)]
        public int FileSizeLimitInKB { get; set; }

        public bool ByPassApplyMigrationFileCheck { get; set; }

        [Required, MinLength(3)]
        public string UploadPath { get; set; } = "";

        [Range(3, 20)]
        public int GameCodeInitialLength { get; set; } = 5;
        /// <summary>
        /// This is the top it can get to
        /// </summary>
        [Range(3, 20)]
        public int GameCodeMaxLength { get; set; } = 8;
        /// <summary>
        /// Game states will be deleted after this many hours
        /// </summary>
        [Range(1, int.MaxValue)]
        public int DeleteExpireGamesInHours { get; set; } = 720;

        [Range(1, 1000)]
        public int MaxGamesPerUser { get; set; }

        [Range(1, 5000)]
        public int MetricsServiceWaitForMinutes { get; set; } = 30;

        public int GetFileSizeLimitInBytes()
        {
            return FileSizeLimitInKB * 1024;
        }
    }
}
