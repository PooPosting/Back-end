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
    public Comment GetById(int commId)
    {
        return _dbContext.Comments
            .Include(c => c.Picture)
            .Include(c => c.Author)
            .AsSplitQuery()
            .SingleOrDefault(c => c.Id == commId)!;
    }

    public int Insert(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        return comment.Id;
    }

    public void Update(Comment comment)
    {
        _dbContext.Comments.Update(comment);
    }

    public void DeleteById(int id)
    {
        var comm =  _dbContext.Comments.SingleOrDefault(c => c.Id == id);
        comm!.IsDeleted = true;
    }
    
    public bool Save()
    {
        return _dbContext.SaveChanges() > 0;
    }
}