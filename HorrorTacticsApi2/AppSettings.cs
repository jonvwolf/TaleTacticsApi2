using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2
{
    public class AppSettings
    {
        [Required, MinLength(2)]
        public string MainPassword { get; set; } = "";
        [Range(64, 49152)]
        public int FileSizeLimitInKB { get; set; }

        public bool ByPassApplyMigrationFileCheck { get; set; }

        [Required, MinLength(1)]
        public string UploadPath { get; set; } = "";

        public int GetFileSizeLimitInBytes()
        {
            return FileSizeLimitInKB * 1024;
        }
    }
}
