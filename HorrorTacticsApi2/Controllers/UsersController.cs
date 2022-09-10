using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Games;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Domain.Models.Users;
using HorrorTacticsApi2.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HorrorTacticsApi2.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route($"{Constants.SecuredApiPath}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UsersController : ControllerBase
    {
        readonly UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadUserModel>> Get([FromRoute] long id, CancellationToken token)
        {
            var model = await _service.TryGetAsync(id, token);
            if (model == default)
                return NotFound();

            return Ok(model);
        }

        //[HttpGet("scenes/{idStoryScene}/[controller]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Produces(MediaTypeNames.Application.Json)]
        //public async Task<ActionResult<List<ReadStorySceneCommandModel>>> GetAll([FromRoute] long idStoryScene, CancellationToken token)
        //{
        //    var model = await _service.GetAllAsync(idStoryScene, true, token);

        //    return Ok(model);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadUserModel>> Post([FromBody] CreateUserModel model, CancellationToken token)
        {
            var dto = await _service.CreateAsync(model, token);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ReadStorySceneCommandModel>> Put([FromRoute] long id, [FromBody] UpdateUserModel model, CancellationToken token)
        {
            var dto = await _service.UpdateAsync(id, model, token);
            return Ok(dto);
        }

        //[HttpDelete("scenes/[controller]/{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Produces(MediaTypeNames.Application.Json)]
        //public async Task<ActionResult> Delete([FromRoute] long id, CancellationToken token)
        //{
        //    await _service.DeleteCommandAsync(id, token);
        //    return NoContent();
        //}
    }
}
