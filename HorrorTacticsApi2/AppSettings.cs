using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2
{
    public class AppSettings
    {
        [Required, MinLength(2)]
        public string MainPassword { get; set; } = "";
        [Range(1, 1000)]
        public int MaxImageSizeInMegabytes { get; set; }
    }
}
