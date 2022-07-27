using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HorrorTacticsApi2.Controllers
{
    [Authorize]
    [Route($"{Constants.SecuredApiPath}")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class StorySceneCommandsController : ControllerBase
    {
        readonly StorySceneCommandsService _service;
        public StorySceneCommandsController(StorySceneCommandsService service)
        {
            _service = service;
        }

        [HttpGet("storyscenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Get([FromRoute] long id, CancellationToken token)
        {
            var model = await _service.TryGetAsync(id, true, token);
            if (model == default)
                return NotFound();

            return Ok(model);
        }

        [HttpGet("storyscenes/{idStoryScene}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<ReadStorySceneCommandModel>>> GetAll([FromRoute] long idStoryScene, CancellationToken token)
        {
            var model = await _service.GetAllAsync(idStoryScene, true, token);
            if (model == default)
                return NotFound();

            return Ok(model);
        }

        [HttpPost("storyscenes/{idStoryScene}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Post([FromRoute] long idStoryScene, [FromBody] CreateStorySceneCommandModel model, CancellationToken token)
        {
            var dto = await _service.CreateCommandAsync(idStoryScene, model, true, token);
            return Ok(dto);
        }

        [HttpPut("storyscenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Put([FromRoute] long id, [FromBody] UpdateStorySceneCommandModel model, CancellationToken token)
        {
            var dto = await _service.UpdateCommandAsync(id, model, true, token);
            return Ok(dto);
        }

        [HttpDelete("storyscenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Delete([FromRoute] long id, CancellationToken token)
        {
            await _service.DeleteCommandAsync(id, token);
            return NoContent();
        }
    }
}
