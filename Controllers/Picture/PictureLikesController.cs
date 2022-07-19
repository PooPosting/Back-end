using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Picture;

[ApiController]
[Route("api/picture/{picId}/like")]
public class PictureLikesController: ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPictureLikingService _pictureLikingService;

    public PictureLikesController(
        IPictureService pictureService,
        IPictureLikingService pictureLikingService
        )
    {
        _pictureService = pictureService;
        _pictureLikingService = pictureLikingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPictureLikes(
        [FromRoute] string picId
        )
    {
        // var likes = await _pictureService.GetPicLikes(IdHasher.DecodePictureId(picId));
        // return Ok(likes);
        throw new NotImplementedException(); // make the result paged
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