﻿using System;
using System.Collections.Generic;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountService
{
    AccountDto GetById(Guid id);
    PagedResult<AccountDto> GetAll(AccountQuery query);
    IEnumerable<AccountDto> GetAllOdata();
    bool Update(PutAccountDto dto);
    bool Delete(Guid id);
}