using PooPosting.Api.Entities;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class RestrictedIpsService : IRestrictedIpsService
{
    private readonly IRestrictedIpRepo _restrictedIpRepo;

    public RestrictedIpsService(
        IRestrictedIpRepo restrictedIpRepo)
    {
        _restrictedIpRepo = restrictedIpRepo;
    }
    
    public async Task<IEnumerable<RestrictedIp>> GetAll()
    {
        return await _restrictedIpRepo.GetAllAsync();
    }
    
    public async Task<RestrictedIp> GetByIp(string ip)
    {
        return await _restrictedIpRepo.GetByIpAsync(ip) ?? throw new NotFoundException();
    }
    
    public async Task Add(string ip, bool cantGet, bool cantPost)
    {
        var restrictedIp = new RestrictedIp()
        {
            IpAddress = ip,
            CantGet = cantGet,
            CantPost = cantPost
        };
        await _restrictedIpRepo.InsertAsync(restrictedIp);
    }
    
    public async Task UpdateMany(IEnumerable<RestrictedIp> ips, bool cantGet, bool cantPost)
    {
        foreach (var ip in ips)
        {
            ip.CantGet = cantGet;
            ip.CantPost = cantPost;
            await _restrictedIpRepo.UpdateAsync(ip);
        }
        await RemoveUnnecessaryIps();
    }

    private async Task RemoveUnnecessaryIps()
    {
        var unnecessaryIps =
            (await _restrictedIpRepo.GetAllAsync())
            .Where(r => !r.CantGet && !r.CantPost);
        foreach (var unnecessaryIp in unnecessaryIps)
        {
            await _restrictedIpRepo.DeleteAsync(unnecessaryIp);
        }
    }
}