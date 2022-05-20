using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicturesAPI.Entities;

public class RestrictedIp
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string IpAddress { get; set; }
    public bool Banned { get; set; }
    public bool CantPost { get; set; }
}