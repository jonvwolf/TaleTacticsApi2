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
    public class CommandsController : HtController
    {
        readonly StorySceneCommandsService _service;
        public CommandsController(StorySceneCommandsService service)
        {
            _service = service;
        }

        [HttpGet("scenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Get([FromRoute] long id, CancellationToken token)
        {
            var model = await _service.TryGetAsync(GetUser(), id, true, token);
            if (model == default)
                return NotFound();

            return Ok(model);
        }

        [HttpGet("scenes/{idStoryScene}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<ReadStorySceneCommandModel>>> GetAll([FromRoute] long idStoryScene, CancellationToken token)
        {
            var model = await _service.GetAllAsync(GetUser(), idStoryScene, true, token);
            
            return Ok(model);
        }

        [HttpPost("scenes/{idStoryScene}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Post([FromRoute] long idStoryScene, [FromBody] CreateStorySceneCommandModel model, CancellationToken token)
        {
            var dto = await _service.CreateCommandAsync(GetUser(), idStoryScene, model, true, token);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("scenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Put([FromRoute] long id, [FromBody] UpdateStorySceneCommandModel model, CancellationToken token)
        {
            var dto = await _service.UpdateCommandAsync(GetUser(), id, model, true, token);
            return Ok(dto);
        }

        [HttpDelete("scenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Delete([FromRoute] long id, CancellationToken token)
        {
            await _service.DeleteCommandAsync(GetUser(), id, token);
            return NoContent();
        }
    }
}
