using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Entities;

public class Role
{
    public int Id { get; set; }
    
    [MaxLength(16)]
    public string Name { get; set; }
}