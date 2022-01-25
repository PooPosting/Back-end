using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Account GetAccountById(Guid id);
    Account GetAccountByNick(string nickname);
    IEnumerable<Account> GetAccounts();
    Guid CreateAccount(Account newAccount);
    void UpdateAccount(PutAccountDto dto, string id);
    void DeleteAccount(Guid id);
    string GetLikedTags(Guid accId);
    void AddLikedTags(Account acc, Picture picture);
    void RemoveLikedTags(Account acc, Picture picture);

}