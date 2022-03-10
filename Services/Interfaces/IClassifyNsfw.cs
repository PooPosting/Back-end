namespace PicturesAPI.Services.Interfaces;

public interface IClassifyNsfw
{
    bool IsSafeForWork(string picId);
}