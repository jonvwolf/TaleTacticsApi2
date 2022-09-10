using HorrorTacticsApi2.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HorrorTacticsApi2.Controllers
{
    public abstract class HtController : ControllerBase
    {
        UserJwt? userJwt;

        public UserJwt GetUser()
        {
            if (userJwt == default)
            {
                var user = HttpContext.User;
                if (user != default && user.Claims != default)
                {
                    var userId = GetClaim(JwtRegisteredClaimNames.Sub, user.Claims);
                    var username = GetClaim(JwtRegisteredClaimNames.UniqueName, user.Claims);
                    var role = GetClaim(Constants.JwtRoleKey, user.Claims);

                    if (!Enum.TryParse(role.Value, out UserRole roleEnum))
                        roleEnum = UserRole.NotSet;

                    userJwt = new UserJwt(long.Parse(userId.Value), username.Value, roleEnum);
                }
            }

            if (userJwt == default)
                throw new InvalidOperationException("userJwt is default");
            return userJwt;
        }

        static Claim GetClaim(string type, IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(x => x.Type == type);
            if (claim == default)
                throw new InvalidOperationException("Jwt does not have: " + type);

            if (string.IsNullOrEmpty(claim.Value))
                throw new InvalidOperationException("Jwt claim value is null or empty: " + type);

            return claim;
        }
    }
}
