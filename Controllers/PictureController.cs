using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

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

    // only for testing
    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("odata")]
    public ActionResult<PagedResult<PictureDto>> GetAllOData()
    {
        var pictures = _pictureService.GetAllOdata();
        return Ok(pictures);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public ActionResult<PictureDto> GetSinglePictureById([FromRoute] Guid id)
    {
        var picture = _pictureService.GetById(id);
        return Ok(picture);
    }

    [HttpPost]
    [Route("create")]
    public ActionResult PostPicture([FromBody] CreatePictureDto dto)
    {
        var pictureId = _pictureService.Create(dto);
        
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult PutPictureUpdate([FromRoute] Guid id, [FromBody] PutPictureDto dto)
    {
        _pictureService.Put(id, dto);
        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/voteup")]
    public ActionResult PatchPictureVoteUp([FromRoute] Guid id)
    {
        var likeOperationResult = _pictureLikingService.Like(id);
        return Ok(likeOperationResult);
    }
        
    [HttpPatch]
    [Route("{id}/votedown")]
    public ActionResult PatchPictureVoteDown([FromRoute] Guid id)
    {
        var likeOperationResult = _pictureLikingService.DisLike(id);
        return Ok(likeOperationResult);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeletePicture([FromRoute] Guid id)
    {
        _pictureService.Delete(id);
        return NoContent();
    }

}