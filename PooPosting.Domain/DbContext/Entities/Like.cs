using System.ComponentModel.DataAnnotations;

namespace PooPosting.Domain.DbContext.Entities;

public class Like
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int PictureId { get; set; }
    public DateTime Liked { get; set; } = DateTime.Now;
    public Account Account { get; set; } = null!;
    public Picture Picture { get; set; } = null!;
}