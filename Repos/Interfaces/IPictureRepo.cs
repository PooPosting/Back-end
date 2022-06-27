#nullable enable
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    Task<Picture?> GetByIdAsync(int id);
    Task<IEnumerable<Picture>> GetFromAllAsync(int itemsToSkip, int itemsToTake);
    Task<IEnumerable<Picture>> GetNotSeenByAccountIdAsync(int accountId, int itemsToTake);
    Task<IEnumerable<Picture>> SearchAllAsync(int itemsToSkip, int itemsToTake, string searchPhrase);
    Task<IEnumerable<Picture>> SearchNewestAsync(int itemsToSkip, int itemsToTake, string searchPhrase);
    Task<IEnumerable<Picture>> SearchMostLikesAsync(int itemsToSkip, int itemsToTake, string searchPhrase);
    Task<Picture> InsertAsync(Picture picture);
    Task<Picture> UpdatePicScoreAsync (Picture picture);
    Task<Picture> UpdateAsync(Picture picture);
    Task<bool> DeleteByIdAsync(int id);
}