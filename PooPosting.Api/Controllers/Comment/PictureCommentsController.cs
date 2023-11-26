using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;

namespace PooPosting.Api.Controllers.Comment;

[ApiController]
[Route("api/picture/{picId}/comment")]
public class PictureCommentsController(
    ICommentService commentService
    ) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetPictureComments(
        [FromRoute] string picId,
        [FromQuery] Query query
        )
    {
        var result = await commentService.GetByPictureId(IdHasher.DecodePictureId(picId), query);
        return Ok(result);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostComment(
        [FromRoute] string picId,
        [FromBody] PostPutCommentDto dto
        )
    {
        var result = await commentService.Create(IdHasher.DecodePictureId(picId), dto.Text);
        return Ok(result);
    }


}