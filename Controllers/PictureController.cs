using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.ActionFilters;
using PicturesAPI.Configuration;
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
    public IActionResult GetAllPictures([FromQuery] PictureQuery query)
    {
        var pictures = _pictureService.GetAll(query);
        return Ok(pictures);
    }
    
    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("search")]
    public IActionResult SearchAllPictures([FromQuery] SearchQuery query)
    {
        var pictures = _pictureService.SearchAll(query);
        return Ok(pictures);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public IActionResult GetSinglePictureById([FromRoute] string id)
    {
        var picture = _pictureService.GetById(IdHasher.DecodePictureId(id));
        return Ok(picture);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likes")]
    public IActionResult GetPictureLikes([FromRoute] string id)
    {
        var likes = _pictureService.GetPicLikes(IdHasher.DecodePictureId(id));
        return Ok(likes);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likers")]
    public IActionResult GetPictureLikers([FromRoute] string id)
    {
        var likes = _pictureService.GetPicLikers(IdHasher.DecodePictureId(id));
        return Ok(likes);
    }

    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [ServiceFilter(typeof(CanGetFilter))]
    [Route("classify")]
    public IActionResult ClassifyPicture([FromForm] IFormFile file)
    {
        var result = _pictureService.Classify(file);
        return Ok(result);
    }
    
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [ServiceFilter(typeof(CanGetFilter))]
    [Route("create")]
    public IActionResult PostPicture(
        [FromForm] IFormFile file, 
        [FromForm] string name, 
        [FromForm] string description)
        // [FromForm] string[] tags)
    {
        var dto = new CreatePictureDto()
        {
            Name = name,
            Description = description,
            // Tags = tags.ToList()
        };
        
        var pictureId = _pictureService.Create(file, dto);
        
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpPut]
    [ServiceFilter(typeof(CanPostFilter))]
    [ServiceFilter(typeof(CanGetFilter))]
    [Route("{id}")]
    public IActionResult PutPictureUpdate([FromRoute] string id, [FromBody] PutPictureDto dto)
    {
        var result = _pictureService.Update(IdHasher.DecodePictureId(id), dto);
        return Ok(result);
    }

    [HttpPatch]
    [Route("{id}/voteup")]
    public IActionResult PatchPictureVoteUp([FromRoute] string id)
    {
        var result = _pictureLikingService.Like(IdHasher.DecodePictureId(id));
        return Ok(result);
    }
        
    [HttpPatch]
    [Route("{id}/votedown")]
    public IActionResult PatchPictureVoteDown([FromRoute] string id)
    {
        var result = _pictureLikingService.DisLike(IdHasher.DecodePictureId(id));
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeletePicture([FromRoute] string id)
    {
        _pictureService.Delete(IdHasher.DecodePictureId(id));
        return NoContent();
    }

}