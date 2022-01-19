using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
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
    [Route("{id}")]
    public ActionResult<AccountDto> GetAccountById([FromRoute] Guid id)
    {
        var account = _accountService.GetById(id);
        return Ok(account);
    }
    
    [HttpGet]
    [EnableQuery]
    public ActionResult<PagedResult<AccountDto>> GetAllAccounts([FromQuery] AccountQuery query)
    {
        var accounts = _accountService.GetAll(query);
        return Ok(accounts);
    }

    [HttpGet]
    [EnableQuery]
    [Route("odata")]
    public ActionResult<IEnumerable<AccountDto>> GetAllOdata()
    {
        var result = _accountService.GetAllOdata();
        return Ok(result);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult UpdateAccount([FromBody] PutAccountDto dto)
    {
        _accountService.Update(dto);
        return NoContent();
    }

    [HttpDelete] 
    [Route("{id}")]
    public ActionResult DeleteAccount([FromRoute] Guid id)
    {
        _accountService.Delete(id);
        return NoContent();
    }

    [HttpPost]
    [Route("register")]
    public ActionResult PostAccount([FromBody] CreateAccountDto dto)
    {
        var accountId = _userAccountService.Create(dto);
        return Created($"api/accounts/{accountId}", null);
    }
        
    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public ActionResult Login([FromBody] LoginDto dto)
    {
        string token = _userAccountService.GenerateJwt(dto);
        return Ok(token);
    }
        

        
}