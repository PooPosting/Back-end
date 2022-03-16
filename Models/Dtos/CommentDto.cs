namespace PicturesAPI.Models.Dtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; } = DateTime.Now;
    public string AuthorNickname { get; set; }
    public Guid PictureId { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsModifiable { get; set; } = false;
}