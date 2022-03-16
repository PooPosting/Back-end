using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Account GetAccountById(Guid id, DbInclude option);
    Account GetAccountByNick(string nickname, DbInclude option);
    IEnumerable<Account> GetAccounts(DbInclude option);
    Guid CreateAccount(Account newAccount);
    void UpdateAccount(PutAccountDto dto, Guid id);
    void DeleteAccount(Guid id);
    string GetLikedTags(Guid accId);
    void AddLikedTags(Account acc, Picture picture);
    void RemoveLikedTags(Account acc, Picture picture);
    bool Exists(Guid id);

}