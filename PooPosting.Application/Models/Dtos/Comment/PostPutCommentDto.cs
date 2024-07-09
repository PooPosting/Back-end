using System.ComponentModel.DataAnnotations;

namespace PooPosting.Application.Models.Dtos.Comment;

public class PostPutCommentDto
{
    [Required]
    public string Text { get; set; }
}