namespace PicturesAPI.Services.Helpers.Interfaces;

public interface IPictureHelper
{
    Task<bool> MarkAsSeenAsync(int accountId, int pictureId);
}