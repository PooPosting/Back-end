using System;
using System.Collections.Generic;
using PicturesAPI.Models;

namespace PicturesAPI.Interfaces;

public interface IAccountService
{
    AccountDto GetAccountById(Guid id);
    IEnumerable<AccountDto> GetAllAccounts();
    void UpdateAccount(PutAccountDto dto);
    void DeleteAccount(Guid id);
}