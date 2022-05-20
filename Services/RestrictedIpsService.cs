using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class RestrictedIpsService: IRestrictedIpsService
{
    private readonly IRestrictedIpRepo _restrictedIpRepo;
    private readonly IAccountContextService _accountContextService;

    public RestrictedIpsService(
        IRestrictedIpRepo restrictedIpRepo,
        IAccountContextService accountContextService)
    {
        _restrictedIpRepo = restrictedIpRepo;
        _accountContextService = accountContextService;
    }
    
    public List<RestrictedIp> GetAllRestrictedIps()
    {
        return _restrictedIpRepo.GetRestrictedIps();
    }
    
    public RestrictedIp GetRestrictedIp(string ip)
    {
        return _restrictedIpRepo.GetRestrictedIp(ip) ?? throw new NotFoundException("There is no such a restricted ip");
    }
    
    public bool AddRestrictedIp(string ip, bool banned, bool cantPost)
    {
        return _restrictedIpRepo.AddRestrictedIp(ip, banned, cantPost);
    }
    
    public bool UpdateRestrictedIps(List<string> ips, bool? banned, bool? cantPost)
    {
        var changeBans = banned is not null;
        var changePosts = cantPost is not null;
        
        foreach (var ip in ips)
        {
            if (changeBans)
            {
                _restrictedIpRepo.RestrictIpBan(ip, banned.Value);
            }
            if (changePosts)
            {
                _restrictedIpRepo.RestrictIpBan(ip, cantPost.Value);
            }
        }
        RemoveUnnecessaryIps();
        return true;
    }

    private void RemoveUnnecessaryIps()
    {
        var unnecessaryIps = _restrictedIpRepo.GetRestrictedIps().Where(r => !r.Banned && !r.CantPost);
        foreach (var unnecessaryIp in unnecessaryIps)
        {
            _restrictedIpRepo.DeleteRestrictedIp(unnecessaryIp.IpAddress);
        }
    }
}