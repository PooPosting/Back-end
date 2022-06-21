namespace PicturesAPI.Models.Interfaces;

public interface IModifiable
{
    public bool IsModifiable { get; set; }
    public string AccountId { get; set; }
}