using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;

namespace PooPosting.Api.Services.Interfaces;

public interface IPictureService
{
    Task<PictureDto> GetById(int id);
    Task<IEnumerable<PictureDto>> GetPictures(PersonalizedQuery query);
    Task<PagedResult<PictureDto>> GetPictures(Query query);
    Task<PagedResult<PictureDto>> SearchAll(CustomQuery query);
    Task<PictureDto> UpdatePictureName(int picId, UpdatePictureNameDto dto);
    Task<PictureDto> UpdatePictureDescription(int picId, UpdatePictureDescriptionDto dto);
    Task<PictureDto> UpdatePictureTags(int picId, UpdatePictureTagsDto dto);
    Task<string> Create(CreatePictureDto dto);
    Task<bool> Delete(int id);
}
