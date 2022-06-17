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
    public Task<Comment> GetById(Guid commId)
    {
        return _dbContext.Comments
            .Include(c => c.Picture)
            .Include(c => c.Author)
            .AsSplitQuery()
            .SingleOrDefaultAsync(c => c.Id == commId)!;
    }

    public async Task<Guid> Insert(Comment comment)
    {
        await _dbContext.Comments.AddAsync(comment);
        return comment.Id;
    }

    public async Task Update(Comment comment)
    {
        _dbContext.Update(comment);
    }
    
    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}