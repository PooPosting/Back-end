using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class RestrictedIpRepo: IRestrictedIpRepo
{
    private readonly PictureDbContext _dbContext;

    public RestrictedIpRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public  RestrictedIp GetByIp(string ip)
    {
        return _dbContext.RestrictedIps.FirstOrDefault(i => i.IpAddress == ip);
    }
    public  List<RestrictedIp> GetAll()
    {
        return _dbContext.RestrictedIps.ToList();
    }

    public  void Insert(RestrictedIp restrictedIp)
    {
        _dbContext.RestrictedIps.Add(restrictedIp);
    }
    public  void Update(RestrictedIp restrictedIp)
    {
        _dbContext.RestrictedIps.Update(restrictedIp);
    }
    public  void Drop(RestrictedIp restrictedIp)
    {
        _dbContext.RestrictedIps.Remove(restrictedIp);
    }

    public  bool Save()
    {
        return _dbContext.SaveChanges() > 0;
    }

}