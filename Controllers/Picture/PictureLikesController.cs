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
    public async Task<IActionResult> GetPictureLikes([FromRoute] string picId)
    {
        var likes = await _pictureService.GetPicLikes(IdHasher.DecodePictureId(picId));
        return Ok(likes);
    }

    [HttpGet]
    [Route("account")]
    public async Task<IActionResult> GetPictureLikers([FromRoute] string picId)
    {
        var likes = await _pictureService.GetPicLikers(IdHasher.DecodePictureId(picId));
        return Ok(likes);
    }

    [HttpPatch]
    [Route("voteup")]
    public async Task<IActionResult> PatchPictureVoteUp([FromRoute] string picId)
    {
        var result = await _pictureLikingService.Like(IdHasher.DecodePictureId(picId));
        return Ok(result);
    }

    [HttpPatch]
    [Route("votedown")]
    public async Task<IActionResult> PatchPictureVoteDown([FromRoute] string picId)
    {
        var result = await _pictureLikingService.DisLike(IdHasher.DecodePictureId(picId));
        return Ok(result);
    }
}