using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Models;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Models.Queries;

namespace PooPosting.Application.Services.Interfaces;

public interface IPictureService
{
    Task<PictureDto> GetById(int id);
    Task<PagedResult<PictureDto>> GetAll(Query query);
    Task<PagedResult<PictureDto>> GetAll(PictureSearchQuery query);
    Task<PagedResult<PictureDto>> GetTrending(Query query);
    Task<PictureDto> UpdateName(int picId, UpdatePictureNameDto dto);
    Task<PictureDto> UpdateDescription(int picId, UpdatePictureDescriptionDto dto);
    Task<PictureDto> UpdateTags(int picId, UpdatePictureTagsDto dto);
    Task<string> Create(CreatePictureDto dto);
    Task<bool> Delete(int id);
}
