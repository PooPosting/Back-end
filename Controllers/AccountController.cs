using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.ActionFilters;
using PicturesAPI.Configuration;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;
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
    [AllowAnonymous]
    [Route("{id}")]
    public IActionResult GetAccountById([FromRoute] string id)
    {
        var account = _accountService.GetById(IdHasher.DecodeAccountId(id));
        return Ok(account);
    }
    
    // [HttpGet]
    // [Route("likedTags")]
    // public IActionResult GetTags()
    // {
    //     var tags = _accountService.GetLikedTags();
    //     return Ok(tags);
    // }
    
    [HttpGet]
    [AllowAnonymous]
    [EnableQuery]
    public IActionResult GetAllAccounts([FromQuery] AccountQuery query)
    {
        var accounts = _accountService.GetAll(query);
        return Ok(accounts);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateAccount([FromBody] PutAccountDto dto)
    {
        var result = _accountService.Update(dto);
        return Ok(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likes")]
    public IActionResult GetPictureLikes([FromRoute] string id)
    {
        var likes = _accountService.GetAccLikes(IdHasher.DecodeAccountId(id));
        return Ok(likes);
    }

    [HttpDelete] 
    [Route("{id}/all-pictures")]
    public IActionResult DeleteAccount([FromRoute] string id)
    {
        var result = _accountService.DeleteAccPics(IdHasher.DecodeAccountId(id));
        return Ok(result);
    }
    
    [HttpDelete] 
    [Route("{id}")]
    public IActionResult DeleteAccountPictures([FromRoute] string id)
    {
        var result = _accountService.Delete(IdHasher.DecodeAccountId(id));
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [AllowAnonymous]
    [Route("register")]
    public IActionResult PostAccount([FromBody] CreateAccountDto dto)
    {
        var accountId = _userAccountService.Create(dto);
        return Created($"api/accounts/{accountId}", null);
    }
        
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var result = _userAccountService.GenerateJwt(dto);
        return Ok(result);
    }
    
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [AllowAnonymous]
    [Route("verifyJwt")]
    public IActionResult VerifyJwt([FromBody] LsLoginDto dto)
    {
        var result = _userAccountService.VerifyJwt(dto);
        return Ok(result);
    }
        

        
}