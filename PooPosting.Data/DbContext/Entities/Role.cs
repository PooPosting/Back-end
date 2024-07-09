using System.ComponentModel.DataAnnotations;
using PooPosting.Data.DbContext.Interfaces;

namespace PooPosting.Data.DbContext.Entities;

public class Role: IIdentifiable
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string Name { get; set; } = null!;
}