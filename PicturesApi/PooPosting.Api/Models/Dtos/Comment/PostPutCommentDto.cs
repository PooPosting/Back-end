using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Comment;

public class PostPutCommentDto
{
    [Required]
    public string Text { get; set; }
}