using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services.Interfaces;

public interface IPopularService
{
    Task<PopularContentDto> Get();
}