using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.Models.Dtos.Picture;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture/{picId}/update")]
public class PictureUpdateController : ControllerBase
{
    private readonly IPictureService _pictureService;

    public PictureUpdateController(
        IPictureService pictureService
        )
    {
        _pictureService = pictureService;
    }

    [HttpPatch]
    [Route("name")]
    public async Task<IActionResult> UpdatePictureName(
        [FromRoute] string picId,
        [FromBody] UpdatePictureNameDto dto
    )
    {
        return Ok(await _pictureService.UpdatePictureName(IdHasher.DecodePictureId(picId), dto));
    }

    [HttpPatch]
    [Route("description")]
    public async Task<IActionResult> UpdatePictureDescription(
        [FromRoute] string picId,
        [FromBody] UpdatePictureDescriptionDto dto
    )
    {
        return Ok(await _pictureService.UpdatePictureDescription(IdHasher.DecodePictureId(picId), dto));
    }

    [HttpPatch]
    [Route("tags")]
    public async Task<IActionResult> UpdatePictureTags(
        [FromRoute] string picId,
        [FromBody] UpdatePictureTagsDto dto
    )
    {
        return Ok(await _pictureService.UpdatePictureTags(IdHasher.DecodePictureId(picId), dto));
    }
}