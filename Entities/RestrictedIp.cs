using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicturesAPI.Entities;

public class RestrictedIp
{
    [Key]
    public int Id { get; set; }

    [MaxLength(128)]
    public string IpAddress { get; set; }

    public bool CantGet { get; set; }
    public bool CantPost { get; set; }
}