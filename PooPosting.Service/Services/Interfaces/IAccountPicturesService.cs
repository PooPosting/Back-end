using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Models.Dtos.Picture;

namespace PooPosting.Service.Services.Interfaces;

public interface IAccountPicturesService
{
    Task<PagedResult<PictureDto>> GetPaged(PaginationParameters paginationParameters, string accountId);
    Task<PagedResult<PictureDto>> GetLikedPaged(PaginationParameters paginationParameters, string accountId);
}