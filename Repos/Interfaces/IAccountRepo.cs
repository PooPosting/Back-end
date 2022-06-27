#nullable enable
using System.Linq.Expressions;
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Task<int> CountAccountsAsync(Expression<Func<Account, bool>> predicate);
    Task<Account?> GetByIdAsync(int id);
    Task<Account?> GetByNickAsync(string nickname);
    Task<IEnumerable<Account>> SearchAllAsync(int itemsToSkip, int itemsToTake, string searchPhrase);
    Task<Account> InsertAsync(Account account);
    Task<Account> UpdateAsync(Account account);
    Task<bool> TryDeleteByIdAsync(int id);
    Task<bool> MarkAsSeenAsync(int accountId, int pictureId);
}