using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Models;

namespace PooPosting.Application.Services.Interfaces;

public interface ILikeService
{
    Task<PagedResult<LikeDto>> GetLikesByPictureId(Query query, int picId);
}