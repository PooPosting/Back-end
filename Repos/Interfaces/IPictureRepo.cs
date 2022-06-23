using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    Picture GetById(int id);
    IEnumerable<Picture> GetFromAll(int itemsToSkip, int itemsToTake);
    IEnumerable<Picture> GetNotSeenByAccountId(int accountId, int itemsToTake);
    IEnumerable<Picture> SearchAll(int itemsToSkip, int itemsToTake, string itemSearchPhrase);
    IEnumerable<Picture> SearchNewest(int itemsToSkip, int itemsToTake, string itemSearchPhrase);
    IEnumerable<Picture> SearchMostLikes(int itemsToSkip, int itemsToTake, string itemSearchPhrase);
    int Insert(Picture picture);
    void UpdatePicScore(Picture picture);
    void Update(Picture picture);
    void DeleteById(int id);
    bool Save();
}