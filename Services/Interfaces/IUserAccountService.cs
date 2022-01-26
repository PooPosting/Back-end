using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IUserAccountService
{
    Guid Create(CreateAccountDto dto);
    LoginSuccessResult GenerateJwt(LoginDto dto);
}