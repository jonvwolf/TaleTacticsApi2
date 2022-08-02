using HorrorTacticsApi2.Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HorrorTacticsApi2.Domain.Models.Audio
{
    public record ReadAudioModel(
        long Id,
        string Name,
        bool IsBgm,
        uint DurationSeconds,
        [JsonConverter(typeof(StringEnumConverter))]
        FileFormatEnum Format, 
        string AbsoluteUrl,
        bool IsScanned,
        long size);
}
