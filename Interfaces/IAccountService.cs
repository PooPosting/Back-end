using System;
using System.Collections.Generic;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Interfaces;

public interface IAccountService
{
    AccountDto GetById(Guid id);
    PagedResult<AccountDto> GetAll(AccountQuery query);
    IEnumerable<AccountDto> GetAllOdata();
    void Update(PutAccountDto dto);
    void Delete(Guid id);
}