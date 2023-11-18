using Microsoft.AspNetCore.Mvc;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AccountAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] CreateAccountDto dto)
    {
        var accountId = await _authService.RegisterAccount(dto);
        return Created($"/api/account/{accountId}", null);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginWithAuthCredsDto withAuthCredsDto)
    {
        var result = await _authService.GenerateJwt(withAuthCredsDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshJwt([FromBody] LoginWithRefreshTokenDto dto)
    {
        var result = await _authService.GenerateJwt(dto);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("forgetTokens")]
    public async Task<IActionResult> ForgetTokens([FromBody] ForgetTokensDto dto)
    {
        await _authService.Forget(dto);
        return Ok();
    }

}
