using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Configuration;
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
    [ServiceFilter(typeof(CanPostFilter))]
    [ServiceFilter(typeof(CanGetFilter))]
    public IActionResult PostComment([FromRoute] string id, [FromBody] PostPutCommentDto text)
    {
        var result = _pictureCommentService.Create(IdHasher.DecodeCommentId(id), text.Text);
        return Created($"api/picture/{result.PictureId}",result);
    }
    
    [HttpPatch]
    [ServiceFilter(typeof(CanPostFilter))]
    [ServiceFilter(typeof(CanGetFilter))]
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