using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
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
    public ActionResult<PagedResult<PictureDto>> GetAllPictures([FromQuery] PictureQuery query)
    {
        var pictures = _pictureService.GetAll(query);
        return Ok(pictures);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public IActionResult GetSinglePictureById([FromRoute] Guid id)
    {
        var picture = _pictureService.GetById(id);
        return Ok(picture);
    }

    [HttpPost]
    [Route("create")]
    public IActionResult PostPicture(
        [FromForm] IFormFile file, 
        [FromForm] string name, 
        [FromForm] string description, 
        [FromForm] string[] tags)
    {
        var dto = new CreatePictureDto()
        {
            Name = name,
            Description = description,
            Tags = tags.ToList()
        };
        
        var pictureId = _pictureService.Create(file, dto);
        
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult PutPictureUpdate([FromRoute] Guid id, [FromBody] PutPictureDto dto)
    {
        var result = _pictureService.Put(id, dto);
        return Ok(result);
    }

    [HttpPatch]
    [Route("{id}/voteup")]
    public IActionResult PatchPictureVoteUp([FromRoute] Guid id)
    {
        var likeOperationResult = _pictureLikingService.Like(id);
        return Ok(likeOperationResult);
    }
        
    [HttpPatch]
    [Route("{id}/votedown")]
    public IActionResult PatchPictureVoteDown([FromRoute] Guid id)
    {
        var likeOperationResult = _pictureLikingService.DisLike(id);
        return Ok(likeOperationResult);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeletePicture([FromRoute] Guid id)
    {
        var result = _pictureService.Delete(id);
        return Ok(result);
    }

}