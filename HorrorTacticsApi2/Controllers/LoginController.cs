using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HorrorTacticsApi2.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class LoginController : HtController
    {
        readonly IJwtGenerator _jwt;
        readonly UserService _userService;

        public LoginController(IJwtGenerator jwt, UserService userService)
        {
            _jwt = jwt;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TokenModel>> Post([FromBody] LoginModel model, CancellationToken cancellationToken)
        {
            var user = await _userService.TryGetAsync(model.Password, model.Username, cancellationToken, updateLastLogin: true);
            if (user == default)
                return NotFound();
            
            var jwt = _jwt.GenerateJwtSecurityToken(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(Constants.JwtRoleKey, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            string token = _jwt.SerializeToken(jwt);
            return Ok(new TokenModel(token));
        }
    }
}
