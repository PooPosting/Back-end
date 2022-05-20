using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IRestrictedIpRepo
{
    bool IsIpRestricted(string ip);
    RestrictedIp GetRestrictedIp(string ip);
    List<RestrictedIp> GetRestrictedIps();
    bool RestrictIpBan(string ip, bool ban);
    bool RestrictIpPost(string ip, bool restrictPosting);
    bool DeleteRestrictedIp(string ip);
    bool AddRestrictedIp(string ip, bool banned, bool cantPost);
}