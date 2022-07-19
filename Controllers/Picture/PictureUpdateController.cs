using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PicturesAPI.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture/{picId}/update")]
public class PictureUpdateController : ControllerBase
{
    // use models instead of simple types in 'fromforms' and validate them.

    [HttpPatch]
    [Route("name")]
    public async Task<IActionResult> UpdatePictureName(
        [FromRoute] string picId,
        [FromForm] string name
    )
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("description")]
    public async Task<IActionResult> UpdatePictureDescription(
        [FromRoute] string picId,
        [FromForm] string description
    )
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("tags")]
    public async Task<IActionResult> UpdatePictureTags(
        [FromRoute] string picId,
        [FromForm] string tags
    )
    {
        throw new NotImplementedException();
    }
}