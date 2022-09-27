using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HorrorTacticsApi2.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class FilesController : ControllerBase
    {
        readonly AudiosService service;
        readonly ImagesService image;

        public FilesController(AudiosService service, ImagesService image)
        { 
            this.service = service;
            this.image = image;
        }

        [HttpGet(Constants.FileImageApiPathWithVars)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Image.Jpeg)]
        public async Task<IActionResult> GetImage(
            [FromRoute, 
            StringLength(ValidationConstants.File_Filename_MaxStringLength, MinimumLength = ValidationConstants.File_Name_MinStringLength), 
            Required] string filename, CancellationToken token)
        {
            // TODO: check filename is validated
            // TODO: add some basic regex to the filename
            try
            {
                var stream = await image.GetStreamFileAsync(filename, true, token);
                // stream is disposed by the framework
                return File(stream, MediaTypeNames.Image.Jpeg, filename);
            }
            catch (HtNotFoundException)
            {
                // TODO: In integration tests it will Error controller will not kick in...
                return NotFound();
            }
        }

        [HttpGet(Constants.FileAudioApiPathWithVars)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("audio/mpeg")]
        public async Task<IActionResult> GetAudio(
            [FromRoute,
            StringLength(ValidationConstants.File_Filename_MaxStringLength, MinimumLength = ValidationConstants.File_Name_MinStringLength),
            Required] string filename, CancellationToken token)
        {
            // TODO: check filename is validated
            // TODO: add some basic regex to the filename
            try
            {
                var stream = await service.GetStreamFileAsync(filename, true, token);
                // stream is disposed by the framework
                return File(stream, "audio/mpeg", filename);
            }
            catch (HtNotFoundException)
            {
                // TODO: In integration tests it will Error controller will not kick in...
                return NotFound();
            }
        }
    }
}
