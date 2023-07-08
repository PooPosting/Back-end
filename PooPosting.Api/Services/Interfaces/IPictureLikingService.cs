using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services.Interfaces;

public interface IPictureLikingService
{
    Task<PictureDto> Like(int id);
    Task<PictureDto> DisLike(int id);
}