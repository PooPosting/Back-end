using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAccount(CreateAccountDto dto);
    Task<AuthSuccessResult> GenerateJwt(LoginWithRefreshTokenDto dto);
    Task<AuthSuccessResult> GenerateJwt(LoginWithAuthCredsDto dto);
    Task Forget(ForgetTokensDto dto);
}