using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Comment;
using PicturesAPI.Models.Queries;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture/{picId}/comment")]
public class PictureCommentsController: ControllerBase
{
    private readonly IPictureCommentService _pictureCommentService;

    public PictureCommentsController(
        IPictureCommentService pictureCommentService)
    {
        _pictureCommentService = pictureCommentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPictureComments(
        [FromRoute] string picId,
        [FromBody] Query query
        )
    {
        throw new NotImplementedException();
    }


    [HttpPost]
    public async Task<IActionResult> PostComment(
        [FromRoute] string picId,
        [FromBody] PostPutCommentDto dto
        )
    {
        var result = await _pictureCommentService.Create(IdHasher.DecodePictureId(picId), dto.Text);
        return Ok(result);
    }


}