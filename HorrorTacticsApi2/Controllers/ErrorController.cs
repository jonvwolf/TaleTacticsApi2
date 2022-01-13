using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HorrorTacticsApi2.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        public ErrorController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpDelete, HttpHead, HttpPut, HttpPost, HttpGet, HttpHead, HttpOptions, HttpPatch]
        [Route("/error")]
        public IActionResult Error() => Handle();

        IActionResult Handle()
        {
            // TODO: log when null
            var requestFeature = _httpContextAccessor.HttpContext?.Features.Get<IHttpRequestFeature>();
            if (requestFeature == default)
                return Problem();

            if (requestFeature.RawTarget == "/error")
                return Ok();

            var exceptionFeature = _httpContextAccessor.HttpContext?.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature == default)
                return Problem();

            if (exceptionFeature.Error is HtException htEx)
                return Problem(htEx.Message, statusCode: htEx.StatusCode, title: "An error occurred while processing your request.");

            return Problem();
        }
    }
}
