using PicturesAPI.Entities;

namespace PicturesAPI.Models.Dtos;

public class PatchRestrictedIp
{
    public List<RestrictedIp> Ips { get; set; }
    public bool CantGet { get; set; }
    public bool CantPost { get; set; }
}