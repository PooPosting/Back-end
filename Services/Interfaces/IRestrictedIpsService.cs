using PicturesAPI.Entities;

namespace PicturesAPI.Services.Interfaces;

public interface IRestrictedIpsService
{
    Task<List<RestrictedIp>> GetAll();
    Task<RestrictedIp> GetByIp(string ip);
    Task Add(string ip, bool cantGet, bool cantPost);
    Task UpdateMany(List<RestrictedIp> ips, bool cantGet, bool cantPost);
}