#nullable enable
using PooPosting.Api.Entities;

namespace PooPosting.Api.Repos.Interfaces;

public interface IRestrictedIpRepo
{
    Task<RestrictedIp?> GetByIpAsync(string ip);
    Task<IEnumerable<RestrictedIp>> GetAllAsync();
    Task<RestrictedIp> InsertAsync(RestrictedIp restrictedIp);
    Task<RestrictedIp> UpdateAsync(RestrictedIp restrictedIp);
    Task<RestrictedIp> DeleteAsync(RestrictedIp restrictedIp);
}