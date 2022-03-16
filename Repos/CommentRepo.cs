#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class CommentRepo : ICommentRepo
{
    private readonly PictureDbContext _dbContext;

    public CommentRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Comment? GetComment(Guid commId)
    {
        return _dbContext.Comments
            .Include(c => c.Picture)
            .Include(c => c.Author)
            .AsSplitQuery()
            .SingleOrDefault(c => c.Id == commId);
    }
    public Guid CreateComment(Guid picId, Guid authorId, string text)
    {
        var comment = new Comment()
        {
            Author = _dbContext.Accounts.SingleOrDefault(a => a.Id == authorId),
            Picture = _dbContext.Pictures.SingleOrDefault(p => p.Id == picId),
            Text = text
        };
        _dbContext.Comments.Add(comment);
        _dbContext.SaveChanges();
        return comment.Id;
    }

    public bool ModifyComment(Guid commId, string text)
    {
        var commToModify = _dbContext.Comments.SingleOrDefault(c => c.Id == commId);
        if (commToModify is null) return false; 
        commToModify.Text = text;
        _dbContext.SaveChanges();
        return true;
    }
    
    public bool DeleteComment(Guid commId)
    {
        var commToDelete = _dbContext.Comments.SingleOrDefault(c => c.Id == commId);
        if (commToDelete is null) throw new NotFoundException("comment not found");
        _dbContext.Comments.Remove(commToDelete);
        _dbContext.SaveChanges();
        return true;
    }
    
    public bool Exists(Guid id)
    {
        var comment = _dbContext.Comments.SingleOrDefault(c => c.Id == id);
        return (comment is not null);
    }
}