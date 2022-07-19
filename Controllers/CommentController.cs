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
    private readonly IPictureCommentService _pictureCommentService;

    public CommentController(
        IPictureCommentService pictureCommentService)
    {
        _pictureCommentService = pictureCommentService;
    }

    [HttpPatch]
    [Route("{commId}")]
    public async Task<IActionResult> PatchComment(
        [FromRoute] string commId,
        [FromBody] PostPutCommentDto text
        )
    {
        var result = await _pictureCommentService.Update(IdHasher.DecodeCommentId(commId), text.Text);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{commId}")]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] string commId
        )
    {
        await _pictureCommentService.Delete(IdHasher.DecodeCommentId(commId));
        return NoContent();
    }
}