#nullable enable
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
    public async Task<Comment?> GetByIdAsync(int commId)
    {
        return await _dbContext.Comments
            .Include(c => c.Picture)
            .Include(c => c.Account)
            .AsSplitQuery()
            .SingleOrDefaultAsync(c => c.Id == commId);
    }

    public async Task<Comment> InsertAsync(Comment comment)
    {
        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();

        return _dbContext.Comments
            .Select(c => new Comment()
            {
                Id = c.Id,
                Text = c.Text,
                CommentAdded = c.CommentAdded,
                Account = new Account()
                {
                    Id = c.Account.Id,
                    Nickname = c.Account.Nickname
                },
                Picture = new Picture()
                {
                    Id = c.Picture.Id
                },
            }).SingleOrDefault(c => c.Id == comment.Id)!;
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> TryDeleteByIdAsync(int id)
    {
        var comment =  _dbContext.Comments.SingleOrDefault(c => c.Id == id);
        if (comment is not null)
        {
            comment.IsDeleted = true;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }
    
}