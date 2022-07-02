using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; }
    public bool IsDeleted { get; set; }

    // navigation props
    [Required] public Account Account { get; set; }
    [Required] public Picture Picture { get; set; }
}