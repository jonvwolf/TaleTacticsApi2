using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HorrorTacticsApi2.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class GamesController : HtController
    {
        readonly GamesService games;
        public GamesController(GamesService games)
        { 
            this.games = games;
        }

        [AllowAnonymous]
        [HttpGet("[controller]/{gameCode}/configuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<ReadGameConfiguration> GetConfiguration([FromRoute] string gameCode)
        {
            // This endpoint is for the hub clients (not the horror master)
            return Ok(games.GetGameConfiguration(gameCode));
        }

        [HttpGet($"{Constants.SecuredApiPath}/[controller]/" + "{gameCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<ReadGameStateModel> Get([FromRoute] string gameCode)
        {
            var item = games.GetAllGames(GetUser()).SingleOrDefault(x => x.Code == gameCode);
            if (item == default)
                return NotFound();

            return Ok(item);
        }

        [HttpGet($"{Constants.SecuredApiPath}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<List<ReadGameStateModel>> GetAll()
        {
            return Ok(games.GetAllGames(GetUser()));
        }

        [HttpDelete($"{Constants.SecuredApiPath}/[controller]/" + "{gameCode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult Delete([FromRoute] string gameCode)
        {
            games.DeleteGame(GetUser(), gameCode);
            return NoContent();
        }
    }
}
