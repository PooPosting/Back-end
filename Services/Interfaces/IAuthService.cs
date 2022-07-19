using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Account;

namespace PicturesAPI.Services.Interfaces;

public interface IAuthService
{
    Task<AccountDto> RegisterAccount(CreateAccountDto dto);
    Task<LoginSuccessResult> GenerateJwt(LoginDto dto);
    Task<LoginSuccessResult> VerifyJwt(LsLoginDto dto);
}