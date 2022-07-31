using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Models.Queries;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Account;

[ApiController]
[Route("api/account/{accId}/picture")]
public class AccountPicturesController: ControllerBase
{
    private readonly IPictureService _pictureService;

    public AccountPicturesController(
        IPictureService pictureService
        )
    {
        _pictureService = pictureService;
    }

    [HttpGet]
    [EnableQuery]
    [Route("posted")]
    public async Task<IActionResult> GetPostedPictures(
        [FromRoute] string accId,
        [FromQuery] Query query
    )
    {
        var result = await _pictureService.GetPostedPictures(
            IdHasher.DecodeAccountId(accId), query
        );
        return Ok(result);
    }

    [HttpGet]
    [EnableQuery]
    [Route("liked")]
    public async Task<IActionResult> GetLikedPictures(
        [FromRoute] string accId,
        [FromQuery] Query query
        )
    {
        var result = await _pictureService.GetLikedPictures(
            IdHasher.DecodeAccountId(accId), query
        );
        return Ok(result);
    }
}