using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/picture/{picId}/comment")]
public class CommentController: ControllerBase
{
    private readonly IPictureCommentService _pictureCommentService;

    public CommentController(
        IPictureCommentService pictureCommentService)
    {
        _pictureCommentService = pictureCommentService;
    }
    
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    public IActionResult PostComment([FromRoute] string picId, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.Create(IdHasher.DecodePictureId(picId), text.Text);
        return Created($"api/picture/{result.PictureId}",result);
    }
    
    [HttpPatch]
    [ServiceFilter(typeof(CanPostFilter))]
    [Route("{commId}")]
    public IActionResult PatchComment([FromRoute] string commId, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.Update(IdHasher.DecodeCommentId(commId), text.Text);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{commId}")]
    public IActionResult DeleteComment([FromRoute] string commId)
    {
        _pictureCommentService.Delete(IdHasher.DecodeCommentId(commId));
        return NoContent();
    }
}