using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPopularService
{
    PopularContentDto GetPopularContent();
}