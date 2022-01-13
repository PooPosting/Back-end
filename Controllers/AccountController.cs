using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;

namespace PicturesAPI.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IUserAccountService _userAccountService;

    public AccountController(IAccountService accountService, IUserAccountService userAccountService)
    {
        _accountService = accountService;
        _userAccountService = userAccountService;
    }
        
    [HttpGet]
    [EnableQuery]
    public ActionResult<List<AccountDto>> GetAllAccounts()
    {
        var accounts = _accountService.GetAllAccounts();
        return Ok(accounts);
    }
        
    [HttpGet]
    [EnableQuery]
    [Route("{id}")]
    public ActionResult<AccountDto> GetAccountById([FromRoute] Guid id)
    {
        var account = _accountService.GetAccountById(id);
        return Ok(account);
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public ActionResult UpdateAccount([FromBody] PutAccountDto dto)
    {
        _accountService.UpdateAccount(dto);
        return NoContent();
    }

    [HttpDelete] 
    [Authorize]
    [Route("{id}")]
    public ActionResult DeleteAccount([FromRoute] Guid id)
    {
        _accountService.DeleteAccount(id);
        return NoContent();
    }

    [HttpPost]
    [Route("register")]
    public ActionResult PostAccount([FromBody] CreateAccountDto dto)
    {
        var accountId = _userAccountService.CreateAccount(dto);
        return Created($"api/accounts/{accountId}", null);
    }
        
    [HttpPost]
    [Route("login")]
    public ActionResult Login([FromBody] LoginDto dto)
    {
        string token = _userAccountService.GenerateJwt(dto);
        return Ok(token);
    }
        

        
}