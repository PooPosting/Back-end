using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;
using PicturesAPI.Services;

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
    public ActionResult<List<PictureDto>> GetAllPictures()
    {
        var pictures = _pictureService.GetAllPictures();
        return Ok(pictures);
    }

    [HttpGet]
    [EnableQuery]        
    [AllowAnonymous]
    [Route("{id}")]
    public ActionResult<PictureDto> GetSinglePictureById([FromRoute] Guid id)
    {
        var picture = _pictureService.GetSinglePictureById(id);
        return Ok(picture);
    }

    [HttpPost]
    [Route("create")]
    public ActionResult PostPicture([FromBody] CreatePictureDto dto)
    {
        var pictureId = _pictureService.CreatePicture(dto);
        
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult PutPictureUpdate([FromRoute] Guid id, [FromBody] PutPictureDto dto)
    {
        _pictureService.PutPicture(id, dto);
        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/voteup")]
    public ActionResult PatchPictureVoteUp([FromRoute] Guid id)
    {
        var likeOperationResult = _pictureLikingService.LikePicture(id);
        return Ok(likeOperationResult);
    }
        
    [HttpPatch]
    [Route("{id}/votedown")]
    public ActionResult PatchPictureVoteDown([FromRoute] Guid id)
    {
        var likeOperationResult = _pictureLikingService.DisLikePicture(id);
        return Ok(likeOperationResult);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeletePicture([FromRoute] Guid id)
    {
        _pictureService.DeletePicture(id);
        return NoContent();
    }

}