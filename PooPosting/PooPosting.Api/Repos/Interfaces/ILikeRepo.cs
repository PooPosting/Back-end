#nullable enable
using System.Linq.Expressions;
using PooPosting.Api.Entities;

namespace PooPosting.Api.Repos.Interfaces;

public interface ILikeRepo
{
    Task<Like?> GetByIdAsync(int id);
    Task<int> CountLikesAsync(Expression<Func<Like, bool>> predicate);
    Task<IEnumerable<Like>> GetByPictureIdAsync(int picId, int itemsToSkip, int itemsToTake);
}