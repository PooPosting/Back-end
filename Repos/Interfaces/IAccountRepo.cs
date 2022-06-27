#nullable enable
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Task<Account?> GetByIdAsync(int id);
    Task<Account?> GetByNickAsync(string nickname);
    Task<IEnumerable<Account>> SearchAllAsync(int itemsToSkip, int itemsToTake, string searchPhrase);
    Task<Account> InsertAsync(Account account);
    Task<Account> UpdateAsync(Account account);
    Task<bool> TryDeleteByIdAsync(int id);
    Task<bool> MarkAsSeenAsync(int accountId, int pictureId);
}