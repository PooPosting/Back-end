#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Models.Queries;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Account;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> SearchAllAccounts(
        [FromQuery] CustomQuery query
    )
    {
        var accounts = await _accountService.GetAll(query);
        return Ok(accounts);
    }

    [HttpGet]
    [Route("{accId}")]
    public async Task<IActionResult> GetAccountById(
        [FromRoute] string accId
        )
    {
        var account = await _accountService.GetById(IdHasher.DecodeAccountId(accId));
        return Ok(account);
    }

    [HttpDelete]
    [Route("{accId}")]
    public async Task<IActionResult> DeleteAccount(
        [FromRoute] string accId
    )
    {
        return Ok(await _accountService.Delete(IdHasher.DecodeAccountId(accId)));
    }

}