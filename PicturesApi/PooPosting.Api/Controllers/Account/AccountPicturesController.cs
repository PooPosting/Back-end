using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Controllers.Account;

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