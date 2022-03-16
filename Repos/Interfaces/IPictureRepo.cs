using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    IEnumerable<Picture> GetPictures();
    IEnumerable<Picture> GetPicturesByOwner(Account account);
    Picture GetPictureById(Guid id);
    Guid CreatePicture(Picture picture);
    bool UpdatePicture(Picture picture, PutPictureDto dto);
    bool DeletePicture(Picture picture);
    bool Exists(Guid id);
}