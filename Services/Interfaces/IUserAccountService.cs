using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IUserAccountService
{
    bool Create(CreateAccountDto dto);
    LoginSuccessResult GenerateJwt(LoginDto dto);
    LoginSuccessResult VerifyJwt(LsLoginDto dto);
}