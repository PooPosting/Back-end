#nullable enable
using System.Linq.Expressions;
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IAccountRepo
{
    Task<int> CountAccountsAsync(
        Expression<Func<Account, bool>> filterExp);
    Task<int> CountAccountsAsync();
    Task<Account?> GetByIdAsync(int id);
    Task<Account?> GetByIdAsync(int id,
        params Expression<Func<Account, object>>[] includes);
    Task<Account?> GetByNickAsync(
        string nickname,
        params Expression<Func<Account, object>>[] includes);
    Task<IEnumerable<Account>> SearchAllAsync(
        int itemsToSkip,
        int itemsToTake,
        string? searchPhrase,
        params Expression<Func<Account, object>>[] includes);

    Task<Account> InsertAsync(Account account);
    Task<Account> UpdateAsync(Account account);
    Task<bool> MarkAsSeenAsync(int accountId, int pictureId);
}