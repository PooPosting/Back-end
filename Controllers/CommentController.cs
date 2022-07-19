using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Comment;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController: ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(
        ICommentService commentService
        )
    {
        _commentService = commentService;
    }

    [HttpPatch]
    [Route("{commId}")]
    public async Task<IActionResult> PatchComment(
        [FromRoute] string commId,
        [FromBody] PostPutCommentDto text
        )
    {
        var result = await _commentService.Update(IdHasher.DecodeCommentId(commId), text.Text);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{commId}")]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] string commId
        )
    {
        await _commentService.Delete(IdHasher.DecodeCommentId(commId));
        return NoContent();
    }
}