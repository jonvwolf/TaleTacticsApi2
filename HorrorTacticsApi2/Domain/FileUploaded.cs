using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Domain
{
    public record FileUploaded(string Name, long SizeInBytes, FileFormatEnum Format, string Filename);
}
