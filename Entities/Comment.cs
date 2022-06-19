using System.ComponentModel.DataAnnotations;
using PicturesAPI.Entities.Interfaces;

namespace PicturesAPI.Entities;

public class Comment: IDeletable
{
    [Key]
    public int Id { get; set; }

    [Required] [MinLength(4)] [MaxLength(500)]
    public string Text { get; set; }

    public DateTime CommentAdded { get; set; } = DateTime.Now;
    
    [Required]
    public Account Author { get; set; }

    [Required]
    public Picture Picture { get; set; }

    public bool IsDeleted { get; set; } = false;
}