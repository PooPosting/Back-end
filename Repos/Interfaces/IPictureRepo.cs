using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    Picture GetById(int id);
    List<Picture> GetAll();
    List<Picture> GetNotSeenByAccountId(int accountId);
    List<Picture> GetByAccountId(int accountId);
    int Insert(Picture picture);
    void Update(Picture picture);
    void DeleteById(int id);
    bool Save();
}