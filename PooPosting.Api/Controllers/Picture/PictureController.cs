#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Models.Dtos.Picture;
using PooPosting.Service.Models.Queries;
using PooPosting.Service.Services.Helpers;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Api.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture")]
public class PictureController(
    IPictureService pictureService
    ) 
    : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    public async Task<IActionResult> GetPictures(
        [FromQuery] PaginationParameters paginationParameters
        )
    {
        var pictures = await pictureService.GetAll(paginationParameters);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [Route("trending")]
    public async Task<IActionResult> GetTrendingPictures(
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var pictures = await pictureService.GetAll(paginationParameters);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("search")]
    public async Task<IActionResult> SearchAllPictures(
        [FromQuery] PictureQueryParams paginationParameters
        )
    {
        var pictures = await pictureService.GetAll(paginationParameters);
        return Ok(pictures);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public async Task<IActionResult> GetSinglePictureById([FromRoute] string id)
    {
        var picture = await pictureService.GetById(IdHasher.DecodePictureId(id));
        return Ok(picture);
    }

    [HttpPost]
    [Route("post")]
    public async Task<IActionResult> PostPicture([FromForm] CreatePictureDto dto)
    {
        var pictureId = await pictureService.Create(dto);
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeletePicture(
        [FromRoute] string id
        )
    {
        await pictureService.Delete(IdHasher.DecodePictureId(id));
        return NoContent();
    }

}