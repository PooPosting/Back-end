using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Picture;

namespace PicturesAPI.Services.Interfaces;

public interface IPopularService
{
    Task<PopularContentDto> Get();
}