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

    public List<Role> GetAll()
    {
        return _dbContext.Roles.ToList();
    }

    public Role GetById(int id)
    {
        return _dbContext.Roles.FirstOrDefault(r => r.Id == id);
    }
}