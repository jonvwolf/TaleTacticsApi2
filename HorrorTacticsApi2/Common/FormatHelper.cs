namespace HorrorTacticsApi2.Data.Entities
{
    public static class FormatHelper
    {
        // TODO: Test that they both match
        public static readonly IReadOnlyDictionary<string, FileFormatEnum> AllowedImageExtensionsForUpload = new Dictionary<string, FileFormatEnum>()
        {
            { "." + FileFormatEnum.JPEG.ToString().ToLowerInvariant(), FileFormatEnum.JPEG },
            { "." + FileFormatEnum.JPG.ToString().ToLowerInvariant(), FileFormatEnum.JPG }
        };
    }
}
