using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Task<IEnumerable<Account>> GetAll();
    Task<Account> GetById(Guid id);
    Task<Guid> Insert(Account account);
    Task Update(Account account);
    Task Delete(Guid id);
    Task Save();

}