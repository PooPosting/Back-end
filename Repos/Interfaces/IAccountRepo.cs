using System;
using System.Collections.Generic;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Account GetAccountById(Guid id);
    Account GetAccountByNick(string nickname);
    IEnumerable<Account> GetAccounts();
    Guid CreateAccount(CreateAccountDto dto);
    bool UpdateAccount(PutAccountDto dto, string id);
    bool DeleteAccount(Account account);
}