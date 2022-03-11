using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    PagedResult<PictureDto> GetAll(PictureQuery query);
    PagedResult<PictureDto> SearchAll(SearchQuery query);
    IEnumerable<PictureDto> GetAllOdata();
    public List<LikeDto> GetPicLikes(Guid id);
    PictureDto GetById(Guid id);
    Guid Create(IFormFile file, CreatePictureDto dto);
    PictureDto Put(Guid id, PutPictureDto dto);
    bool Delete(Guid id);
}