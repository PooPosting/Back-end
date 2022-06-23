﻿using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IUserAccountService
{
    bool Create(CreateAccountDto dto);
    LoginSuccessResult GenerateJwt(LoginDto dto);
    LsLoginSuccessResult VerifyJwt(LsLoginDto dto);
}