#nullable enable
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class CommentRepo : ICommentRepo
{
    private readonly PictureDbContext _dbContext;

    public CommentRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Comment>> GetByPictureIdAsync(
        int picId,
        int itemsToSkip,
        int itemsToTake
    )
    {
        return await _dbContext.Comments
            .Where(c => c.PictureId == picId)
            .OrderByDescending(c => c.CommentAdded)
            .Include(c => c.Account)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<int> CountCommentsAsync(
        Expression<Func<Comment, bool>> predicate
        )
    {
        return await _dbContext.Comments
            .Where(predicate)
            .CountAsync();
    }

    public async Task<Comment?> GetByIdAsync(
        int commId
        )
    {
        return await _dbContext.Comments
            .Include(c => c.Picture)
            .Include(c => c.Account)
            .AsSplitQuery()
            .SingleOrDefaultAsync(c => c.Id == commId);
    }

    public async Task<Comment> InsertAsync(
        Comment comment
        )
    {
        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();

        return _dbContext.Comments
            .Include(c => c.Account)
            .Include(c => c.Picture)
            .SingleOrDefault(c => c.Id == comment.Id)!;
    }

    public async Task<Comment> UpdateAsync(
        Comment comment
        )
    {
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync();
        return comment;
    }

}