using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    Picture GetById(int id);
    List<Picture> GetAll();
    List<Picture> GetByOwner(Account account);
    int Insert(Picture picture);
    void Update(Picture picture);
    void DeleteById(int id);
    bool Save();
}