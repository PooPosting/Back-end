using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    Task<PictureDto> Like(int id);
    Task<PictureDto> DisLike(int id);
}