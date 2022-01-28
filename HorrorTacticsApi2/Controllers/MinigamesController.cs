using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Models.Minigames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HorrorTacticsApi2.Controllers
{
    [Authorize]
    [Route($"{Constants.SecuredApiPath}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class MinigamesController : ControllerBase
    {
        public MinigamesController()
        { 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<IList<ReadMinigameModel>> Get()
        {
            return Ok(new List<ReadMinigameModel>() { new ReadMinigameModel(1, "find_in_image") });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<ReadMinigameModel> Get([FromRoute] long id)
        {
            return Ok(new ReadMinigameModel(1, "find_in_image"));
        }

    }
}
