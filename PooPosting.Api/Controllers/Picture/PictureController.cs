#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Controllers.Picture;

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
        var pictures = await _pictureService.GetPictures(query);
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
    public async Task<IActionResult> GetSinglePictureById([FromRoute] string id)
    {
        var picture = await _pictureService.GetById(IdHasher.DecodePictureId(id));
        return Ok(picture);
    }

    [HttpPost]
    [Route("post")]
    public async Task<IActionResult> PostPicture([FromBody] CreatePictureDto dto)
    {
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