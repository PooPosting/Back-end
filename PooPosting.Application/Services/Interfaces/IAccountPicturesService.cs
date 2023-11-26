using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Models;

namespace PooPosting.Application.Services.Interfaces;

public interface IAccountPicturesService
{
    Task<PagedResult<PictureDto>> GetPaged(Query query, string accountId);
    Task<PagedResult<PictureDto>> GetLikedPaged(Query query, string accountId);
}