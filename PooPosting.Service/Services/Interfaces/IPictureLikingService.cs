using PooPosting.Service.Models.Dtos.Picture;

namespace PooPosting.Service.Services.Interfaces;

public interface IPictureLikingService
{
    Task<PictureDto> Like(int id);
    Task<PictureDto> DisLike(int id);
}