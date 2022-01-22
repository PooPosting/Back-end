using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Interfaces;

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
    [Route("getTags")]
    public ActionResult<string> GetTags ()
    {
        var tags = _accountService.GetLikedTags();
        return Ok(tags);
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
        var result = _accountService.Update(dto);
        return Ok(result);
    }

    [HttpDelete] 
    [Route("{id}")]
    public ActionResult DeleteAccount([FromRoute] Guid id)
    {
        var result = _accountService.Delete(id);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
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
        var token = _userAccountService.GenerateJwt(dto);
        return Ok(token);
    }
        

        
}