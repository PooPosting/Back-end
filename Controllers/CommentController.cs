using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.Models.Dtos;
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
    public IActionResult PostComment([FromRoute] Guid id, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.CreateComment(id, text.Text);
        return Ok(result);
    }
    
    [HttpPatch]
    [Route("{commId}")]
    public IActionResult PatchComment([FromRoute] Guid commId, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.ModifyComment(commId, text.Text);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{commId}")]
    public IActionResult DeleteComment([FromRoute] Guid commId)
    {
        var result = _pictureCommentService.DeleteComment(commId);
        return Ok(result);
    }
}