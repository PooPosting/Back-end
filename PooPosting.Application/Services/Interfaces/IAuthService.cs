using PooPosting.Application.Models;
using PooPosting.Application.Models.Dtos.Account;

namespace PooPosting.Application.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAccount(CreateAccountDto dto);
    Task<AuthSuccessResult> GenerateJwt(LoginWithRefreshTokenDto dto);
    Task<AuthSuccessResult> GenerateJwt(LoginWithAuthCredsDto dto);
    Task Forget(ForgetTokensDto dto);
}