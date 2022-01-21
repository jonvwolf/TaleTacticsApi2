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
    [Authorize]
    [Route($"{Constants.SecuredApiPath}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class StoryScenesController : ControllerBase
    {
        readonly StoryScenesService _service;
        
        public StoryScenesController(StoryScenesService service)
        { 
            _service = service;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneModel>> GetStoryScene([FromRoute] long id, CancellationToken token)
        {
            var model = await _service.TryGetAsync(id, true, token);
            if (model == default)
                return NotFound();

            return Ok(model);
        }
    }
}
