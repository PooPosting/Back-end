using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Picture;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    Task<PictureDto> Like(int id);
    Task<PictureDto> DisLike(int id);
}