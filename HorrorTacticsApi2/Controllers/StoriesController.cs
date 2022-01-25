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
    [Route($"{Constants.SecuredApiPath}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class StoriesController : ControllerBase
    {
        readonly StoriesService service;
        readonly StoryScenesService sceneService;
        readonly GamesService games;

        public StoriesController(StoriesService service, StoryScenesService sceneService, GamesService games)
        { 
            this.service = service;
            this.sceneService = sceneService;
            this.games = games;
        }

        [HttpGet("{storyId}/scenes/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneModel>> GetStoryScene([FromRoute] long storyId, [FromRoute] long id, CancellationToken token)
        {
            // TODO: performance
            var storyModel = await service.TryFindStoryAsync(storyId, false, token);
            if (storyModel == default)
                return NotFound();

            // TODO: TryMethods should always have the `includeAll` parameter
            var model = await sceneService.TryGetAsync(id, true, token);
            if (model == default)
                return NotFound();

            // TODO: check scene belongs to the storyId

            return Ok(model);
        }

        [HttpPost("{storyId}/scenes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneModel>> Post([FromRoute] long storyId, [FromBody] CreateStorySceneModel model, CancellationToken token)
        {
            var readModel = await sceneService.CreateStorySceneAsync(storyId, model, true, token);
            return CreatedAtAction(nameof(Get), new { id = readModel.Id }, readModel);
        }

        [HttpPut("{storyId}/scenes/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneModel>> Put([FromRoute] long storyId, [FromRoute] long id, [FromBody] UpdateStorySceneModel model, CancellationToken token)
        {
            // TODO: performance
            var storyModel = await service.TryFindStoryAsync(storyId, false, token);
            if (storyModel == default)
                return NotFound();

            // TODO: check scene belongs to the storyId

            return Ok(await sceneService.UpdateStorySceneAsync(id, model, true, token));
        }

        [HttpDelete("{storyId}/scenes/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] long storyId, [FromRoute] long id, CancellationToken token)
        {
            // TODO: performance
            var storyModel = await service.TryFindStoryAsync(storyId, false, token);
            if (storyModel == default)
                return NotFound();

            // TODO: check scene belongs to the storyId

            await sceneService.DeleteStorySceneAsync(id, token);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IList<ReadStoryModel>>> Get(CancellationToken token)
        {
            return Ok(await service.GetAllStoriesAsync(token));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStoryModel>> Get([FromRoute] long id, CancellationToken token)
        {
            var model = await service.TryGetAsync(id, token);
            if (model == default)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStoryModel>> Post([FromBody] CreateStoryModel model, CancellationToken token)
        {
            var readModel = await service.CreateStoryAsync(model, true, token);
            return CreatedAtAction(nameof(Get), new { id = readModel.Id }, readModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStoryModel>> Put([FromRoute] long id, [FromBody] UpdateStoryModel model, CancellationToken token)
        {
            return Ok(await service.UpdateStoryAsync(id, model, true, token));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] long id, CancellationToken token)
        {
            await service.DeleteStoryAsync(id, token);
            return NoContent();
        }

        [HttpPost("game")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadGameCreatedModel>> PostGame([FromBody] CreateGameModel model, CancellationToken token)
        {
            var game = await games.CreateGameAsync(model.StoryId, token);
            return CreatedAtAction(nameof(GamesController.GetConfiguration), nameof(GamesController).Replace("Controller", ""), new { gameCode = game.GameCode }, game);
        }
    }
}
