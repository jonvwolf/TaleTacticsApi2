using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Domain.Dtos
{
    public record ReadImageModel(
        long Id, 
        string Name, 
        uint Width, 
        uint Height, 
        FileFormatEnum Format, 
        string AbsoluteUrl,
        bool IsScanned
    );
}
