using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/picture/{id}/comment")]
public class CommentController: ControllerBase
{
    private readonly IPictureCommentService _pictureCommentService;

    public CommentController(
        IPictureCommentService pictureCommentService)
    {
        _pictureCommentService = pictureCommentService;
    }
    
    [HttpPost]
    [ServiceFilter(typeof(IsIpRestrictedFilter))]
    [ServiceFilter(typeof(IsIpBannedFilter))]
    public IActionResult PostComment([FromRoute] string id, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.CreateComment(GuidEncoder.Decode(id), text.Text);
        return Ok(result);
    }
    
    [HttpPatch]
    [ServiceFilter(typeof(IsIpRestrictedFilter))]
    [ServiceFilter(typeof(IsIpBannedFilter))]
    [Route("{commId}")]
    public IActionResult PatchComment([FromRoute] string commId, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.ModifyComment(GuidEncoder.Decode(commId), text.Text);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{commId}")]
    public IActionResult DeleteComment([FromRoute] string commId)
    {
        var result = _pictureCommentService.DeleteComment(GuidEncoder.Decode(commId));
        return Ok(result);
    }
}