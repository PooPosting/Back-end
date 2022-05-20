using PicturesAPI.Entities;

namespace PicturesAPI.Services.Interfaces;

public interface IRestrictedIpsService
{
    List<RestrictedIp> GetAllRestrictedIps();
    RestrictedIp GetRestrictedIp(string ip);
    bool AddRestrictedIp(string ip, bool banned, bool cantPost);
    bool UpdateRestrictedIps(List<string> ips, bool? banned, bool? cantPost);
}