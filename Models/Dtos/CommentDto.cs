using Newtonsoft.Json;
using PicturesAPI.Models.Interfaces;

namespace PicturesAPI.Models.Dtos;

public class CommentDto: IModifiable
{
    public string Id { get; set; }
    [JsonProperty("AuthorId")]
    public string AccountId { get; set; }
    public string AuthorNickname { get; set; }
    public string PictureId { get; set; }
    public bool IsModifiable { get; set; } = false;

    public string Text { get; set; }
    public DateTime CommentAdded { get; set; } = DateTime.Now;
}