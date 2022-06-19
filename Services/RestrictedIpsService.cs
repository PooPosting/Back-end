using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class RestrictedIpsService: IRestrictedIpsService
{
    private readonly IRestrictedIpRepo _restrictedIpRepo;

    public RestrictedIpsService(
        IRestrictedIpRepo restrictedIpRepo)
    {
        _restrictedIpRepo = restrictedIpRepo;
    }
    
    public List<RestrictedIp> GetAll()
    {
        return _restrictedIpRepo.GetAll();
    }
    
    public RestrictedIp GetByIp(string ip)
    {
        return _restrictedIpRepo.GetByIp(ip) ?? throw new NotFoundException("There is no such a restricted ip");
    }
    
    public void Add(string ip, bool cantGet, bool cantPost)
    {
        var restrictedIp = new RestrictedIp()
        {
            IpAddress = ip,
            CantGet = cantGet,
            CantPost = cantPost
        };
        _restrictedIpRepo.Insert(restrictedIp);
        _restrictedIpRepo.Save();
    }
    
    public void UpdateMany(List<RestrictedIp> ips, bool cantGet, bool cantPost)
    {
        foreach (var ip in ips)
        {
            ip.CantGet = cantGet;
            ip.CantPost = cantPost;
            _restrictedIpRepo.Update(ip);
        }
        RemoveUnnecessaryIps();
        _restrictedIpRepo.Save();
    }

    private void RemoveUnnecessaryIps()
    {
        var unnecessaryIps =
            (_restrictedIpRepo.GetAll())
            .Where(r => !r.CantGet && !r.CantPost);
        foreach (var unnecessaryIp in unnecessaryIps)
        {
            _restrictedIpRepo.Drop(unnecessaryIp);
        }
    }
}