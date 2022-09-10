using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2
{
    public record UserJwt(long Id, string Username, UserRole Role)
    {
        public static readonly UserJwt EmptyUserJwt = new(0, string.Empty, UserRole.NotSet);
    }
}
