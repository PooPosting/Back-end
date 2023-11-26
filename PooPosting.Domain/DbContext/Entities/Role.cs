using System.ComponentModel.DataAnnotations;

namespace PooPosting.Domain.DbContext.Entities;

public class Role
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string Name { get; set; } = null!;
}