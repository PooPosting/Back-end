#nullable enable
using PooPosting.Api.Entities;

namespace PooPosting.Api.Repos.Interfaces;

public interface IRoleRepo
{
    Task<List<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
}