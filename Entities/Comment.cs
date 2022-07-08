using System.ComponentModel.DataAnnotations;
using PicturesAPI.Entities.Interfaces;

namespace PicturesAPI.Entities;

public class Comment : IModifiable
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; }
    public bool IsDeleted { get; set; }

    public int PictureId { get; set; }
    public int AccountId { get; set; }

    // navigation props
    [Required] public Account Account { get; set; }
    [Required] public Picture Picture { get; set; }
}