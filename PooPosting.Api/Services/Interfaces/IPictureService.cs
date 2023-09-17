using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;

namespace PooPosting.Api.Services.Interfaces;

public interface IPictureService
{
    Task<PictureDto> GetById(int id);
    Task<IEnumerable<PictureDto>> GetAll(PersonalizedQuery query);
    Task<PagedResult<PictureDto>> GetAll(Query query);
    Task<PagedResult<PictureDto>> GetAll(CustomQuery query);
    Task<PictureDto> UpdateName(int picId, UpdatePictureNameDto dto);
    Task<PictureDto> UpdateDescription(int picId, UpdatePictureDescriptionDto dto);
    Task<PictureDto> UpdateTags(int picId, UpdatePictureTagsDto dto);
    Task<string> Create(CreatePictureDto dto);
    Task<bool> Delete(int id);
}
