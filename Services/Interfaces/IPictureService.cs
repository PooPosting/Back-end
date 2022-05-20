using Google.Cloud.Vision.V1;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    PagedResult<PictureDto> GetAll(PictureQuery query);
    PagedResult<PictureDto> SearchAll(SearchQuery query);
    List<LikeDto> GetPicLikes(Guid id);
    List<AccountDto> GetPicLikers(Guid id);
    PictureDto GetById(Guid id);
    Guid Create(IFormFile file, CreatePictureDto dto);
    SafeSearchAnnotation Classify(IFormFile file);
    PictureDto Put(Guid id, PutPictureDto dto);
    bool Delete(Guid id);
}