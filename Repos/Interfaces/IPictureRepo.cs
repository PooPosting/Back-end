using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    Task<Picture> GetById(Guid id);
    Task<IEnumerable<Picture>> GetAll();
    IEnumerable<Picture> GetByOwner(Account account);
    Task<Guid> Insert(Picture picture);
    Task Update(Picture picture);
    Task Save();
}