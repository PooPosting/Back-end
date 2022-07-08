using Newtonsoft.Json;

namespace PicturesAPI.Models.Dtos;

public class CommentDto
{
    public string Id { get; set; }
    public string PictureId { get; set; }

    public string Text { get; set; }
    public DateTime CommentAdded { get; set; }

    public AccountPreviewDto AccountPreview { get; set; }
    public bool IsModifiable { get; set; } = false;
    public bool IsAdminModifiable { get; set; } = false;
}