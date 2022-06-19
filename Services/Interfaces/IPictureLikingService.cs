using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    PictureDto Like(int id);
    PictureDto DisLike(int id);
}