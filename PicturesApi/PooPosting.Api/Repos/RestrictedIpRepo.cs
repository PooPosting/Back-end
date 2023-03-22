using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Repos.Interfaces;

#nullable enable

namespace PooPosting.Api.Repos;

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
    public async Task<IEnumerable<RestrictedIp>> GetAllAsync()
    {
        return await _dbContext.RestrictedIps.ToArrayAsync();
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