using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PooPosting.Domain.DbContext.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MinLength(2)]
    [MaxLength(250)]
    public string Text { get; set; } = null!;
    
    [Required]
    public DateTime CommentAdded { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsDeleted { get; set; } = false;

    [Required]
    public int PictureId { get; set; }
    
    [Required]
    public int AccountId { get; set; }

    // navigation props
    public Account Account { get; set; } = null!;
    public Picture Picture { get; set; } = null!;
}