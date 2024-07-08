#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;

namespace PooPosting.Api.Controllers.Account;

[ApiController]
[Route("api/account")]
public class AccountController(
    IAccountService accountService
    ) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> SearchAllAccounts(
        [FromQuery] AccountSearchQuery query
    )
    {
        var accounts = await accountService.GetAll(query);
        return Ok(accounts);
    }

    [HttpGet]
    [Route("{accId}")]
    public async Task<IActionResult> GetAccountById(
        [FromRoute] string accId
        )
    {
        var account = await accountService.GetById(IdHasher.DecodeAccountId(accId));
        return Ok(account);
    }
    
    [HttpGet]
    [Authorize]
    [Route("me")]
    public async Task<IActionResult> GetCurrentAccount()
    {
        var account = await accountService.GetCurrent();
        return Ok(account);
    }

    [HttpDelete]
    [Authorize]
    [Route("{accId}")]
    public async Task<IActionResult> DeleteAccount(
        [FromRoute] string accId
    )
    {
        return Ok(await accountService.Delete(IdHasher.DecodeAccountId(accId)));
    }

}