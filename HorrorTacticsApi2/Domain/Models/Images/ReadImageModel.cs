using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HorrorTacticsApi2.Domain.Dtos
{
    public record ReadImageModel(
        long Id, 
        string Name, 
        uint Width, 
        uint Height,
        [JsonConverter(typeof(StringEnumConverter))]
        FileFormatEnum Format, 
        string AbsoluteUrl,
        bool IsScanned,
        long size
    );
}
