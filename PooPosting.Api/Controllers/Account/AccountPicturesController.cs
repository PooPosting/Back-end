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
    public async Task<IActionResult> GetPictureAccounts(
        [FromQuery] Query query,
        [FromRoute] string accId
    )
    {
        var accounts = await _accountPicService.GetPaged(query, accId);
        return Ok(accounts);
    }
}