using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Models.Dtos.Like;

namespace PooPosting.Service.Services.Interfaces;

public interface ILikeService
{
    Task<PagedResult<LikeDto>> GetLikesByPictureId(PaginationParameters paginationParameters, int picId);
}