using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Entities;

public class Like
{
    public int Id { get; set; }
    public bool IsLike { get; set; }
    public int AccountId { get; set; }
    public int PictureId { get; set; }
    public Account Account { get; set; }
    public Picture Picture { get; set; }
}