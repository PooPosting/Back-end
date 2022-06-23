using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    List<Account> GetAll();
    Account GetById(int id);
    Account GetByNick(string nickname);
    void MarkAsSeen(int accountId, int pictureId);
    void Insert(Account account);
    void Update(Account account);
    void DeleteById(int guid);
    bool Save();

}