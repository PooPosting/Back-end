using Google.Cloud.Vision.V1;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    List<PictureDto> GetPictures(PictureQuery query);
    PagedResult<PictureDto> SearchAll(SearchQuery query);
    List<LikeDto> GetPicLikes(int id);
    List<AccountDto> GetPicLikers(int id);
    PictureDto GetById(int id);
    int Create(IFormFile file, CreatePictureDto dto);
    SafeSearchAnnotation Classify(IFormFile file);
    PictureDto Update(int id, PutPictureDto dto);
    void Delete(int id);
}