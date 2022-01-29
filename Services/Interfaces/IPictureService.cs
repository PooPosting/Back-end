using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    PagedResult<PictureDto> GetAll(PictureQuery query);
    IEnumerable<PictureDto> GetAllOdata();
    PictureDto GetById(Guid id);
    Guid Create(IFormFile file, CreatePictureDto dto);
    bool Put(Guid id, PutPictureDto dto);
    bool Delete(Guid id);
}