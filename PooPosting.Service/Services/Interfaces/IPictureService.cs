using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Models.Dtos.Picture;
using PooPosting.Service.Models.Queries;

namespace PooPosting.Service.Services.Interfaces;

public interface IPictureService
{
    Task<PictureDto> GetById(int id);
    Task<PagedResult<PictureDto>> GetAll(PaginationParameters paginationParameters);
    Task<PagedResult<PictureDto>> GetAll(PictureQueryParams paginationParameters);
    Task<PagedResult<PictureDto>> GetTrending(PaginationParameters paginationParameters);
    Task<PictureDto> UpdateName(int picId, UpdatePictureNameDto dto);
    Task<PictureDto> UpdateDescription(int picId, UpdatePictureDescriptionDto dto);
    Task<PictureDto> UpdateTags(int picId, UpdatePictureTagsDto dto);
    Task<string> Create(CreatePictureDto dto);
    Task<bool> Delete(int id);
}
