using PooPosting.Api.Entities;

namespace PooPosting.Api.Models.Dtos;

public class PatchRestrictedIp
{
    public List<RestrictedIp> Ips { get; set; }
    public bool CantGet { get; set; }
    public bool CantPost { get; set; }
}