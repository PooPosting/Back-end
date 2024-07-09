using System.ComponentModel.DataAnnotations;
using PooPosting.Data.DbContext.Interfaces;

namespace PooPosting.Data.DbContext.Entities;

public class Like: IIdentifiable
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int PictureId { get; set; }
    public DateTime Liked { get; set; } = DateTime.UtcNow;
    public Account Account { get; set; } = null!;
    public Picture Picture { get; set; } = null!;
}