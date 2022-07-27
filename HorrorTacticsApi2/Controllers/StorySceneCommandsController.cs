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
        readonly StorySceneCommandsService service;
        
        public StorySceneCommandsController(StorySceneCommandsService service)
        { 
            this.service = service;
        }

        [HttpGet("scenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Get([FromRoute] long storySceneCommandId, CancellationToken token)
        {
            var cmd = await service.TryGetAsync(storySceneCommandId, true, token);
            if (cmd == default)
                return NotFound();

            return Ok(cmd);
        }

        [HttpPost("scenes/{sceneId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Post([FromRoute] long storySceneId, [FromBody] CreateStorySceneCommandModel model, CancellationToken token)
        {
            var readModel = await service.CreateStorySceneCommandAsync(storySceneId, model, true, token);
            return CreatedAtAction(nameof(Get), new { id = readModel.Id }, readModel);
        }

        [HttpPut("scenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneModel>> Put([FromRoute] long id, [FromBody] UpdateStorySceneCommandModel model, CancellationToken token)
        {
            return Ok(await service.UpdateStorySceneCommandAsync(id, model, true, token));
        }

        [HttpDelete("scenes/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] long id, CancellationToken token)
        {
            await service.DeleteStorySceneCommandAsync(id, token);
            return NoContent();
        }

        [HttpGet("scenes/{sceneId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IList<ReadStorySceneCommandModel>>> Get(CancellationToken token)
        {
            return Ok(await service.GetAllAsync(true, token));
        }
    }
}
