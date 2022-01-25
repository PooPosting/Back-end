namespace PicturesAPI.Models.Dtos;

public class PictureDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountNickname { get; set; }
        
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
        
    public string Url { get; set; }
    public DateTime PictureAdded { get; set; }
        
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}