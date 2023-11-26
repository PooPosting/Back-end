using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;

namespace PooPosting.Api.Controllers.Picture;

[ApiController]
[Route("api/picture/{picId}/like")]
public class PictureLikesController(
        IPictureLikingService pictureLikingService,
        ILikeService likeService
        )
    : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetPictureLikes(
        [FromRoute] string picId,
        [FromQuery] Query query
        )
    {
        var likes = await likeService.GetLikesByPictureId(
            query,
            IdHasher.DecodePictureId(picId)
            );
        return Ok(likes);
    }

    [HttpPatch]
    [Authorize]
    [Route("vote-up")]
    public async Task<IActionResult> PatchPictureVoteUp(
        [FromRoute] string picId
        )
    {
        var result = await pictureLikingService.Like(IdHasher.DecodePictureId(picId));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize]
    [Route("vote-down")]
    public async Task<IActionResult> PatchPictureVoteDown(
        [FromRoute] string picId
        )
    {
        var result = await pictureLikingService.DisLike(IdHasher.DecodePictureId(picId));
        return Ok(result);
    }
}