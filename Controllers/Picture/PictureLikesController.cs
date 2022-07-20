using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Models.Queries;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Picture;

[ApiController]
[Route("api/picture/{picId}/like")]
public class PictureLikesController: ControllerBase
{
    private readonly IPictureLikingService _pictureLikingService;
    private readonly ILikeService _likeService;

    public PictureLikesController(
        IPictureLikingService pictureLikingService,
        ILikeService likeService
        )
    {
        _pictureLikingService = pictureLikingService;
        _likeService = likeService;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetPictureLikes(
        [FromRoute] string picId,
        [FromQuery] Query query
        )
    {
        var likes = await _likeService.GetLikesByPictureId(
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
        var result = await _pictureLikingService.Like(IdHasher.DecodePictureId(picId));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize]
    [Route("vote-down")]
    public async Task<IActionResult> PatchPictureVoteDown(
        [FromRoute] string picId
        )
    {
        var result = await _pictureLikingService.DisLike(IdHasher.DecodePictureId(picId));
        return Ok(result);
    }
}