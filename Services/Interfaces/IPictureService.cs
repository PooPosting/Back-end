using PicturesAPI.Models;
using PicturesAPI.Models.Dtos.Picture;
using PicturesAPI.Models.Queries;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    Task<IEnumerable<PictureDto>> GetPersonalizedPictures(PersonalizedQuery query);
    Task<PagedResult<PictureDto>> GetPictures(Query query);
    Task<PagedResult<PicturePreviewDto>> GetLikedPictures(int id, Query query);
    Task<PagedResult<PicturePreviewDto>> GetPostedPictures(int id, Query query);
    Task<PagedResult<PictureDto>> SearchAll(CustomQuery query);
    Task<PictureDto> GetById(int id);
    Task<string> Create(CreatePictureDto dto);
    Task<bool> Delete(int id);
}