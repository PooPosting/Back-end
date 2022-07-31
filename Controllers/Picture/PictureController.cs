#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Picture;
using PicturesAPI.Models.Queries;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture")]
public class PictureController : ControllerBase
{
    private readonly IPictureService _pictureService;

    public PictureController(
        IPictureService pictureService
        )
    {
        _pictureService = pictureService;
    }

    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    public async Task<IActionResult> GetPictures(
        [FromQuery] Query query
        )
    {
        var pictures = await _pictureService.GetPictures(query);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [Route("personalized")]
    public async Task<IActionResult> GetPersonalizedPictures(
        [FromQuery] PersonalizedQuery query
        )
    {
        var pictures = await _pictureService.GetPersonalizedPictures(query);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("search")]
    public async Task<IActionResult> SearchAllPictures(
        [FromQuery] CustomQuery query
        )
    {
        var pictures = await _pictureService.SearchAll(query);
        return Ok(pictures);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public async Task<IActionResult> GetSinglePictureById(
        [FromRoute] string id
        )
    {
        var picture = await _pictureService.GetById(IdHasher.DecodePictureId(id));
        return Ok(picture);
    }

    [HttpPost]
    [Route("post")]
    public async Task<IActionResult> PostPicture(
        // can i somehow pass whole objects like that?
        [FromForm] IFormFile file,
        [FromForm] string name,
        [FromForm] string? description,
        [FromForm] string? tags
        )
    {
        var dto = new CreatePictureDto()
        {
            Name = name,
            Description = description,
            File = file,
            Tags = tags?.Split(' ').ToList()
        };

        // TEMPORARY

        if (dto.Name.Length > 40 || dto.Name.Length < 4)
        {
            throw new BadRequestException("Picture name should be between 4 and 40 characters");
        }

        if (dto.Description is not null)
        {
            if (dto.Description.Length > 500)
            {
                throw new BadRequestException("Description should be shorter than 500 characters");
            }
        }

        if (dto.File.Length > 4000000)
        {
            throw new BadRequestException("File should be smaller than 4mb");
        }

        if (dto.Tags is not null)
        {
            if ((dto.Tags.Count > 4) || dto.Tags.Any(t => t.Length > 25))
            {
                throw new BadRequestException("Maximum tag count is 4 and every tag should be shorter than 25 characters");
            }
        }

        var pictureId = await _pictureService.Create(dto);
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeletePicture(
        [FromRoute] string id
        )
    {
        await _pictureService.Delete(IdHasher.DecodePictureId(id));
        return NoContent();
    }

}