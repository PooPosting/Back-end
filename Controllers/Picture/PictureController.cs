#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
    [Route("create")]
    public async Task<IActionResult> PostPicture(
        [FromForm] IFormFile file,
        [FromForm] string name,
        [FromForm] string? description,
        [FromForm] string? tags
        ) // can i somehow pass whole objects like that?
    {
        var dto = new CreatePictureDto()
        {
            Name = name,
            Description = description,
            File = file,
            Tags = tags?.Split(' ').ToList()
        };
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