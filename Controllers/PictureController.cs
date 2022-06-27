using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.ActionFilters;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/picture")]
public class PictureController : ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPictureLikingService _pictureLikingService;

    public PictureController(IPictureService pictureService, IPictureLikingService pictureLikingService)
    {
        _pictureService = pictureService;
        _pictureLikingService = pictureLikingService;
    }

    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    public async Task<IActionResult> GetPictures([FromQuery] PictureQuery query)
    {
        var pictures = await _pictureService.GetPictures(query);
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]
    [Route("personalized")]
    public async Task<IActionResult> GetPersonalizedPictures([FromQuery] PictureQueryPersonalized query)
    {
        var pictures = await _pictureService.GetPersonalizedPictures(query);
        return Ok(pictures);
    }
    
    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("search")]
    public async Task<IActionResult> SearchAllPictures([FromQuery] SearchQuery query)
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
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likes")]
    public async Task<IActionResult> GetPictureLikes([FromRoute] string id)
    {
        var likes = await _pictureService.GetPicLikes(IdHasher.DecodePictureId(id));
        return Ok(likes);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likers")]
    public async Task<IActionResult> GetPictureLikers([FromRoute] string id)
    {
        var likes = await _pictureService.GetPicLikers(IdHasher.DecodePictureId(id));
        return Ok(likes);
    }

    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [Route("classify")]
    public async Task<IActionResult> ClassifyPictureAsync([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        var result = await _pictureService.Classify(file, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [Route("create")]
    public async Task<IActionResult> PostPicture(
        [FromForm] IFormFile file, 
        [FromForm] string name, 
        [FromForm] string description,
        [FromForm] string tags)
    {
        var dto = new CreatePictureDto()
        {
            Name = name,
            Description = description,
        };
        if (tags is not null) dto.Tags = tags.Split(' ').ToList();

        var pictureId = await _pictureService.Create(file, dto);
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpPut]
    [ServiceFilter(typeof(CanPostFilter))]
    [Route("{id}")]
    public async Task<IActionResult> PutPictureUpdate([FromRoute] string id, [FromBody] PutPictureDto dto)
    {
        var result = await _pictureService.Update(IdHasher.DecodePictureId(id), dto);
        return Ok(result);
    }

    [HttpPatch]
    [Route("{id}/voteup")]
    public async Task<IActionResult> PatchPictureVoteUp([FromRoute] string id)
    {
        var result = await _pictureLikingService.Like(IdHasher.DecodePictureId(id));
        return Ok(result);
    }
        
    [HttpPatch]
    [Route("{id}/votedown")]
    public async Task<IActionResult> PatchPictureVoteDown([FromRoute] string id)
    {
        var result = await _pictureLikingService.DisLike(IdHasher.DecodePictureId(id));
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeletePicture([FromRoute] string id)
    {
        await _pictureService.Delete(IdHasher.DecodePictureId(id));
        return NoContent();
    }

}