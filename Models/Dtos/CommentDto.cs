using Newtonsoft.Json;

namespace PicturesAPI.Models.Dtos;

public class CommentDto
{
    public string Id { get; set; }
    [JsonProperty("AuthorId")]
    public string AccountId { get; set; }
    public string AuthorNickname { get; set; }
    public string PictureId { get; set; }

    public string Text { get; set; }
    public DateTime CommentAdded { get; set; } = DateTime.Now;

    public bool IsModifiable { get; set; } = false;
    public bool IsAdminModifiable { get; set; } = false;
}