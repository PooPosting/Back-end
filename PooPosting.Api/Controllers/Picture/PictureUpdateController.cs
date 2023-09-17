using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture/{picId}")]
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