using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Entities;

public class Like
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int PictureId { get; set; }
    public DateTime Liked { get; set; } = DateTime.Now;
    public Account Account { get; set; }
    public Picture Picture { get; set; }
}