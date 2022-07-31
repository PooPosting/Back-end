using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; }
    public bool IsDeleted { get; set; }

    public int PictureId { get; set; }
    public int AccountId { get; set; }

    // navigation props
    public Account Account { get; set; }
    public Picture Picture { get; set; }
}