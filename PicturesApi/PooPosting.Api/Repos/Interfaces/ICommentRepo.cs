#nullable enable
using System.Linq.Expressions;
using PooPosting.Api.Entities;

namespace PooPosting.Api.Repos.Interfaces;

public interface ICommentRepo
{
    Task<int> CountCommentsAsync(Expression<Func<Comment, bool>> predicate);
    Task<IEnumerable<Comment>> GetByPictureIdAsync(int picId, int itemsToSkip, int itemsToTake);
    Task<Comment?> GetByIdAsync(int commId);
    Task<Comment> InsertAsync(Comment comment);
    Task<Comment> UpdateAsync(Comment comment);
}