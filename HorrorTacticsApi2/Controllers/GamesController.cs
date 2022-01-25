using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HorrorTacticsApi2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class GamesController : ControllerBase
    {
        readonly GamesService games;
        public GamesController(GamesService games)
        { 
            this.games = games;
        }

        [HttpGet("{gameCode}/configuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<ReadStorySceneModel> GetConfiguration([FromRoute] string gameCode)
        {
            return Ok(games.GetGameConfiguration(gameCode));
        }
    }
}
