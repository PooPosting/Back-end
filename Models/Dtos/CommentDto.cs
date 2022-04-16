namespace PicturesAPI.Models.Dtos;

public class CommentDto
{
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; } = DateTime.Now;
    public string AuthorNickname { get; set; }
    public string PictureId { get; set; }
    public string AuthorId { get; set; }
    public bool IsModifiable { get; set; } = false;
}