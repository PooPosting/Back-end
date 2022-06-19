using PicturesAPI.Entities;

namespace PicturesAPI.Services.Interfaces;

public interface IRestrictedIpsService
{
    List<RestrictedIp> GetAll();
    RestrictedIp GetByIp(string ip);
    void Add(string ip, bool cantGet, bool cantPost);
    void UpdateMany(List<RestrictedIp> ips, bool cantGet, bool cantPost);
}