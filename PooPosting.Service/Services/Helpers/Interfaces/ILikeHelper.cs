using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Services.Helpers.Interfaces;

public interface ILikeHelper
{
    Task<Picture> DislikeAsync(int picId, int accId);

    Task<Picture> LikeAsync(int picId, int accId);
}