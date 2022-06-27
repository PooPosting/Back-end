using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;
#nullable enable

namespace PicturesAPI.Repos;

public class RestrictedIpRepo : IRestrictedIpRepo
{
    private readonly PictureDbContext _dbContext;

    public RestrictedIpRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RestrictedIp?> GetByIpAsync(string ip)
    {
        return await _dbContext.RestrictedIps.FirstOrDefaultAsync(i => i.IpAddress == ip);
    }
    public async Task<List<RestrictedIp>> GetAllAsync()
    {
        return await _dbContext.RestrictedIps.ToListAsync();
    }

    public async Task<RestrictedIp> InsertAsync(RestrictedIp restrictedIp)
    {
        await _dbContext.RestrictedIps.AddAsync(restrictedIp);
        await _dbContext.SaveChangesAsync();
        return restrictedIp;
    }
    public async Task<RestrictedIp> UpdateAsync(RestrictedIp restrictedIp)
    {
        _dbContext.RestrictedIps.Update(restrictedIp);
        await _dbContext.SaveChangesAsync();
        return restrictedIp;
    }
    public async Task<RestrictedIp> DeleteAsync(RestrictedIp restrictedIp)
    {
        _dbContext.RestrictedIps.Remove(restrictedIp);
        await _dbContext.SaveChangesAsync();
        return restrictedIp;
    }

}