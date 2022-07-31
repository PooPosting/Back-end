using PicturesAPI.Models;
using PicturesAPI.Models.Dtos.Picture;
using PicturesAPI.Models.Queries;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    Task<PictureDto> GetById(int id);
    Task<IEnumerable<PictureDto>> GetPersonalizedPictures(PersonalizedQuery query);
    Task<PagedResult<PictureDto>> GetPictures(Query query);
    Task<PagedResult<PicturePreviewDto>> GetLikedPictures(int accId, Query query);
    Task<PagedResult<PicturePreviewDto>> GetPostedPictures(int accId, Query query);
    Task<PagedResult<PictureDto>> SearchAll(CustomQuery query);
    Task<PictureDto> UpdatePictureName(int picId, UpdatePictureNameDto dto);
    Task<PictureDto> UpdatePictureDescription(int picId, UpdatePictureDescriptionDto dto);
    Task<PictureDto> UpdatePictureTags(int picId, UpdatePictureTagsDto dto);
    Task<string> Create(CreatePictureDto dto);
    Task<bool> Delete(int id);
}
