#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IRoleRepo
{
    Task<List<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
}