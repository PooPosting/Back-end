using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
    public int PictureId { get; set; }
    public int AccountId { get; set; }
    

    // navigation props
    public Account Account { get; set; }
    public Picture Picture { get; set; }
}