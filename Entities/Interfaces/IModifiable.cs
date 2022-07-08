namespace PicturesAPI.Entities.Interfaces;

public interface IModifiable
{
    public int Id { get; set; }
    public int AccountId { get; set; }
}