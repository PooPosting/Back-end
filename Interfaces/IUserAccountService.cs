using System;
using PicturesAPI.Models;

namespace PicturesAPI.Interfaces;

public interface IUserAccountService
{
    Guid CreateAccount(CreateAccountDto dto);
    string GenerateJwt(LoginDto dto);
}