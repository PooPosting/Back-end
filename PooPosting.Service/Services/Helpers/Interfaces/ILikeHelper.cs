using PooPosting.Data.DbContext.Entities;

namespace PooPosting.Service.Services.Helpers.Interfaces;

public interface ILikeHelper
{
    Task<Picture> DislikeAsync(int picId, int accId);

    Task<Picture> LikeAsync(int picId, int accId);
}