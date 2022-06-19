using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    List<Account> GetAll();
    Account GetById(int id);
    Account GetByNick(string nickname);
    int Insert(Account account);
    void Update(Account account);
    void DeleteById(int guid);
    bool Save();

}