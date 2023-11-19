#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Controllers.Account;

[ApiController]
[Route("api/account/{accId}/picture")]
public class AccountPicturesController: ControllerBase
{
    private readonly IAccountPicturesService _accountPicService;

    public AccountPicturesController(
        IAccountPicturesService accountPicService
    )
    {
        _accountPicService = accountPicService;
    }
    
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetAccountPictures(
        [FromQuery] Query query,
        [FromRoute] string accId
    )
    {
        var pictures = await _accountPicService.GetPaged(query, accId);
        return Ok(pictures);
    }
    
    [HttpGet]
    [EnableQuery]
    [Route("liked")]
    public async Task<IActionResult> GetLikedPictures(
        [FromQuery] Query query,
        [FromRoute] string accId
    )
    {
        var accounts = await _accountPicService.GetLikedPaged(query, accId);
        return Ok(accounts);
    }
}