using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class PictureRepo : IPictureRepo
{
    private readonly PictureDbContext _dbContext;

    public PictureRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Picture> GetById(Guid id)
    {
        return await _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(p => p.Account)
            .AsSplitQuery()
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Picture>> GetAll()
    {
        return await _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(p => p.Account)
            .AsSplitQuery()
            .ToListAsync();
    }

    public IEnumerable<Picture> GetByOwner(Account account)
    {
        var pictures = _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(p => p.Account)
            .AsSplitQuery()
            .Where(p => p.Account == account);
        return pictures;
    }

    public async Task<Guid> Insert(Picture picture)
    {
        await _dbContext.Pictures.AddAsync(picture);
        return picture.Id;
    }

    public async Task Update(Picture picture)
    {
        _dbContext.Pictures.Update(picture);
    }
    
    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}