using Google.Cloud.Vision.V1;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    Task<IEnumerable<PictureDto>> GetPersonalizedPictures(PictureQueryPersonalized query);
    Task<PagedResult<PictureDto>> GetPictures(PictureQuery query);
    Task<PagedResult<PictureDto>> SearchAll(SearchQuery query);
    Task<IEnumerable<LikeDto>> GetPicLikes(int id);
    Task<IEnumerable<AccountDto>> GetPicLikers(int id);
    Task<PictureDto> GetById(int id);
    Task<string> Create(IFormFile file, CreatePictureDto dto);
    Task<PictureDto> Update(int id, UpdatePictureDto dto);
    Task<bool> Delete(int id);
}