using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicturesAPI.Entities;

public class RestrictedIp
{
    public int Id { get; set; }
    public string IpAddress { get; set; }

    public bool CantGet { get; set; }
    public bool CantPost { get; set; }
}