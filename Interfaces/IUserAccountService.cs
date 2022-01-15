using System;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Interfaces;

public interface IUserAccountService
{
    Guid Create(CreateAccountDto dto);
    string GenerateJwt(LoginDto dto);
}