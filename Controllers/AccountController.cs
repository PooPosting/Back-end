using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.ActionFilters;
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
    public async Task<IActionResult> GetAccountById([FromRoute] string id)
    {
        var account = await _accountService.GetById(IdHasher.DecodeAccountId(id));
        return Ok(account);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [EnableQuery]
    public async Task<IActionResult> GetAllAccounts([FromQuery] AccountQuery query)
    {
        var accounts = await _accountService.GetAll(query);
        return Ok(accounts);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAccount([FromBody] PutAccountDto dto)
    {
        var result = await _accountService.Update(dto);
        return Ok(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likes")]
    public async Task<IActionResult> GetPictureLikes([FromRoute] string id)
    {
        var likes = await _accountService.GetAccLikes(IdHasher.DecodeAccountId(id));
        return Ok(likes);
    }

    [HttpDelete] 
    [Route("{id}/all-pictures")]
    public async Task<IActionResult> DeleteAccount([FromRoute] string id)
    {
        var result = await _accountService.DeleteAccPics(IdHasher.DecodeAccountId(id));
        return Ok(result);
    }
    
    [HttpDelete] 
    [Route("{id}")]
    public async Task<IActionResult> DeleteAccountPictures([FromRoute] string id)
    {
        var result = await _accountService.Delete(IdHasher.DecodeAccountId(id));
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> PostAccount([FromBody] CreateAccountDto dto)
    {
        var result = await _userAccountService.Create(dto);
        return Ok(result);
    }
        
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _userAccountService.GenerateJwt(dto);
        return Ok(result);
    }
    
    [HttpPost]
    [ServiceFilter(typeof(CanPostFilter))]
    [AllowAnonymous]
    [Route("verifyJwt")]
    public async Task<IActionResult> VerifyJwt([FromBody] LsLoginDto dto)
    {
        var result = await _userAccountService.VerifyJwt(dto);
        return Ok(result);
    }
        

        
}