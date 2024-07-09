using System.ComponentModel.DataAnnotations;

namespace PooPosting.Service.Models.Dtos.Comment;

public class PostPutCommentDto
{
    [Required]
    public string Text { get; set; }
}