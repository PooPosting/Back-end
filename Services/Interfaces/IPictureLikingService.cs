using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    PictureDto Like(Guid id);
    PictureDto DisLike(Guid id);
}