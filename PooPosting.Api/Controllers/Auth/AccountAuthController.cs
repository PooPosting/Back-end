using Microsoft.AspNetCore.Mvc;
using PooPosting.Service.Models.Dtos.Account;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AccountAuthController(
    IAuthService authService
    ) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] CreateAccountDto dto)
    {
        var accountId = await authService.RegisterAccount(dto);
        return Created($"/api/account/{accountId}", null);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginWithAuthCredsDto withAuthCredsDto)
    {
        var result = await authService.GenerateJwt(withAuthCredsDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshJwt([FromBody] LoginWithRefreshTokenDto dto)
    {
        var result = await authService.GenerateJwt(dto);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("forgetTokens")]
    public async Task<IActionResult> ForgetTokens([FromBody] ForgetTokensDto dto)
    {
        await authService.Forget(dto);
        return Ok();
    }

}
