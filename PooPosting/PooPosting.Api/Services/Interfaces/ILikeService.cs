using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Models.Queries;

namespace PooPosting.Api.Services.Interfaces;

public interface ILikeService
{
    Task<PagedResult<LikeDto>> GetLikesByPictureId(Query query, int picId);
}