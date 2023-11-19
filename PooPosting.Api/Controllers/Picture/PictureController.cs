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
using PooPosting.Api.Models.Dtos.Picture.Validators;

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
        var pictures = await _pictureService.GetAll(query);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [Route("personalized")]
    public async Task<IActionResult> GetPersonalizedPictures(
        [FromQuery] PersonalizedQuery query
        )
    {
        var pictures = await _pictureService.GetAll(query);
        return Ok(pictures);
    }
    
    [HttpGet]
    [EnableQuery]
    [Route("trending")]
    public async Task<IActionResult> GetTrendingPictures(
        [FromQuery] Query query
    )
    {
        var pictures = await _pictureService.GetAll(query);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("search")]
    public async Task<IActionResult> SearchAllPictures(
        [FromQuery] SearchQuery query
        )
    {
        var pictures = await _pictureService.GetAll(query);
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
    public async Task<IActionResult> PostPicture([FromForm]CreatePictureDto dto)
    {
        var validator = new CreatePictureDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
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