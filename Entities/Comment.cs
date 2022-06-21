using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PicturesAPI.Models.Interfaces;

namespace PicturesAPI.Entities;

public class Comment: IDeletable
{
    [Key]
    public int Id { get; set; }

    public int AccountId { get; set; }

    [Required] [MinLength(4)] [MaxLength(500)]
    public string Text { get; set; }

    public DateTime CommentAdded { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; } = false;


    // navigation props
    [Required]
    public Account Account { get; set; }

    [Required]
    public Picture Picture { get; set; }


}