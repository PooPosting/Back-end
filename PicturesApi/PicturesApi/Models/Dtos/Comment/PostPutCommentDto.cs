using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Comment;

public class PostPutCommentDto
{
    [Required]
    public string Text { get; set; }
}