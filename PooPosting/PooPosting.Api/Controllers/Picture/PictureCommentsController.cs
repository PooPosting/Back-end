using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Api.ActionFilters;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Controllers.Picture;

[ApiController]
[Route("api/picture/{picId}/comment")]
public class PictureCommentsController: ControllerBase
{
    private readonly ICommentService _commentService;

    public PictureCommentsController(
        ICommentService commentService
        )
    {
        _commentService = commentService;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetPictureComments(
        [FromRoute] string picId,
        [FromQuery] Query query
        )
    {
        var result = await _commentService.GetByPictureId(IdHasher.DecodePictureId(picId), query);
        return Ok(result);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostComment(
        [FromRoute] string picId,
        [FromBody] PostPutCommentDto dto
        )
    {
        var result = await _commentService.Create(IdHasher.DecodePictureId(picId), dto.Text);
        return Ok(result);
    }


}