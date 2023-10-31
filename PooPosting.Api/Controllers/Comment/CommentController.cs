using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PooPosting.Api.ActionFilters;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController: ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
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