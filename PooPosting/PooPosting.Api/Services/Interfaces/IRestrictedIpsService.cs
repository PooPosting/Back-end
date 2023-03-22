using PooPosting.Api.Entities;

namespace PooPosting.Api.Services.Interfaces;

public interface IRestrictedIpsService
{
    Task<IEnumerable<RestrictedIp>> GetAll();
    Task<RestrictedIp> GetByIp(string ip);
    Task Add(string ip, bool cantGet, bool cantPost);
    Task UpdateMany(IEnumerable<RestrictedIp> ips, bool cantGet, bool cantPost);
}