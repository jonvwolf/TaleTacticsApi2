using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Domain.Models.Users
{
    public record ReadUserModel(long Id, string Username, UserRole Role)
    {
    }
}
