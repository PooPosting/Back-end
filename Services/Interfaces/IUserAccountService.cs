using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IUserAccountService
{
    Task<string> Create(CreateAccountDto dto);
    Task<LoginSuccessResult> GenerateJwt(LoginDto dto);
    Task<LoginSuccessResult> VerifyJwt(LsLoginDto dto);
}