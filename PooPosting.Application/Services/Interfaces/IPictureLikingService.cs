using PooPosting.Api.Models.Dtos.Picture;

namespace PooPosting.Application.Services.Interfaces;

public interface IPictureLikingService
{
    Task<PictureDto> Like(int id);
    Task<PictureDto> DisLike(int id);
}