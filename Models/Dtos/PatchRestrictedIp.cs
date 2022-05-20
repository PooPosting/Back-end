namespace PicturesAPI.Models.Dtos;

public class PatchRestrictedIp
{
    public List<string> Ips { get; set; }
    public bool? Banned { get; set; }
    public bool? CantPost { get; set; }
}