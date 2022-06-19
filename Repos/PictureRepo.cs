using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class PictureRepo : IPictureRepo
{
    private readonly PictureDbContext _dbContext;

    public PictureRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Picture GetById(int id)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(p => p.Account)
            .AsSplitQuery()
            .SingleOrDefault(p => p.Id == id);
    }

    public List<Picture> GetAll()
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(p => p.Account)
            .AsSplitQuery()
            .ToList();
    }

    public List<Picture> GetByOwner(Account account)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(p => p.Account)
            .AsSplitQuery()
            .Where(p => p.Account == account)
            .ToList();
    }

    public int Insert(Picture picture)
    {
        _dbContext.Pictures.Add(picture);
        return picture.Id;
    }

    public void Update(Picture picture)
    {
        _dbContext.Pictures.Update(picture);
    }

    public void DeleteById(int id)
    {
        var pic = _dbContext.Pictures.SingleOrDefault(p => p.Id == id);
        pic.IsDeleted = true;
    }
    
    public bool Save()
    {
        return _dbContext.SaveChanges() > 0;
    }
}