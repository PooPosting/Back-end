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
    public IActionResult GetAccountById([FromRoute] Guid id)
    {
        var account = _accountService.GetById(id);
        return Ok(account);
    }
    
    [HttpGet]
    [Route("likedTags")]
    public IActionResult GetTags ()
    {
        var tags = _accountService.GetLikedTags();
        return Ok(tags);
    }
    
    [HttpGet]
    [EnableQuery]
    public IActionResult GetAllAccounts([FromQuery] AccountQuery query)
    {
        var accounts = _accountService.GetAll(query);
        return Ok(accounts);
    }

    [HttpGet]
    [EnableQuery]
    [Route("odata")]
    public IActionResult GetAllOdata()
    {
        var result = _accountService.GetAllOdata();
        return Ok(result);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateAccount([FromBody] PutAccountDto dto)
    {
        var result = _accountService.Update(dto);
        return Ok(result);
    }

    [HttpDelete] 
    [Route("{id}")]
    public IActionResult DeleteAccount([FromRoute] Guid id)
    {
        var result = _accountService.Delete(id);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public IActionResult PostAccount([FromBody] CreateAccountDto dto)
    {
        var accountId = _userAccountService.Create(dto);
        return Created($"api/accounts/{accountId}", null);
    }
        
    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _userAccountService.GenerateJwt(dto);
        return Ok(token);
    }
        

        
}