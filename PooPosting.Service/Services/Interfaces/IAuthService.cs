using PooPosting.Service.Models;
using PooPosting.Service.Models.Dtos.Account;

namespace PooPosting.Service.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAccount(CreateAccountDto dto);
    Task<AuthSuccessResult> GenerateJwt(LoginWithRefreshTokenDto dto);
    Task<AuthSuccessResult> GenerateJwt(LoginWithAuthCredsDto dto);
    Task Forget(ForgetTokensDto dto);
}