#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class RoleRepo : IRoleRepo
{
    private readonly PictureDbContext _dbContext;

    public RoleRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _dbContext.Roles.ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }
}