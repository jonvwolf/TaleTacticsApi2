using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    public class LoginController : ControllerBase
    {
        readonly ImagesService _service;
        readonly IJwtGenerator _jwt;
        readonly IOptionsMonitor<AppSettings> _settings;

        public LoginController(ImagesService service, IJwtGenerator jwt, IOptionsMonitor<AppSettings> settings)
        {
            _service = service;
            _jwt = jwt;
            _settings = settings;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public ActionResult<TokenModel> Post([FromBody] LoginModel model)
        {
            if (!_settings.CurrentValue.MainPassword.Equals(model.Password, StringComparison.Ordinal))
                return Problem("Wrong password", statusCode: StatusCodes.Status401Unauthorized, title: "Unauthorized");

            var jwt = _jwt.GenerateJwtSecurityToken(new List<Claim>());
            string token = _jwt.SerializeToken(jwt);
            return Ok(new TokenModel(token));
        }
    }
}
