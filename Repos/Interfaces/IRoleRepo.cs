using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IRoleRepo
{
    List<Role> GetAll();
    Role GetById(int id);
}