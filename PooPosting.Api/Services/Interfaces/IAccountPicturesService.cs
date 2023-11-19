using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;

namespace PooPosting.Api.Services.Interfaces;

public interface IAccountPicturesService
{
    Task<PagedResult<PictureDto>> GetPaged(Query query, string accountId);
    Task<PagedResult<PictureDto>> GetLikedPaged(Query query, string accountId);
}