using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAccount(CreateAccountDto dto);
    Task<LoginSuccessResult> GenerateJwt(LoginDto dto);
    Task<LoginSuccessResult> VerifyJwt(LsLoginDto dto);
}