using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IRestrictedIpRepo
{
    RestrictedIp GetByIp(string ip);
    List<RestrictedIp> GetAll();
    void Insert(RestrictedIp restrictedIp);
    void Update(RestrictedIp restrictedIp);
    void Drop(RestrictedIp restrictedIp);
    bool Save();
}