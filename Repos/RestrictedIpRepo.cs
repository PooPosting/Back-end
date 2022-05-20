using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class RestrictedIpRepo: IRestrictedIpRepo
{
    private readonly PictureDbContext _dbContext;

    public RestrictedIpRepo(
        PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsIpRestricted(string ip)
    {
        return _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip) is null;
    }

    public RestrictedIp GetRestrictedIp(string ip)
    {
        return _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip);
    }
    
    public List<RestrictedIp> GetRestrictedIps()
    {
        return _dbContext.RestrictedIps.ToList();
    }

    public bool DeleteRestrictedIp(string ip)
    {
        var restrictedIp = _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip)!;
        _dbContext.RestrictedIps.Remove(restrictedIp);
        _dbContext.SaveChanges();
        return true;
    }
    
    public bool RestrictIpBan(string ip, bool ban)
    {
        var restrictedIp = _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip)!;
        restrictedIp.Banned = ban;
        _dbContext.SaveChanges();
        return true;
    }
    
    public bool RestrictIpPost(string ip, bool restrictPosting)
    {
        var restrictedIp = _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip)!;
        restrictedIp.CantPost = restrictPosting;
        _dbContext.SaveChanges();
        return true;
    }

    public bool AddRestrictedIp(string ip, bool banned, bool cantPost)
    {
        var restrictedIp = new RestrictedIp()
        {
            IpAddress = ip,
            Banned = banned,
            CantPost = cantPost
        };
        _dbContext.RestrictedIps.Add(restrictedIp);
        _dbContext.SaveChanges();
        
        return true;
    }
    
    public bool UpdateRestrictedIp(string ip, bool banned, bool cantPost)
    {
        var restrictedIp = _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip)!;
        restrictedIp.Banned = banned;
        restrictedIp.CantPost = cantPost;
        _dbContext.SaveChanges();
        return true;
    }
    
}