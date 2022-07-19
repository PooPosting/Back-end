using Google.Cloud.Vision.V1;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Account;
using PicturesAPI.Models.Dtos.Like;
using PicturesAPI.Models.Dtos.Picture;
using PicturesAPI.Models.Queries;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureService
{
    Task<IEnumerable<PictureDto>> GetPersonalizedPictures(PersonalizedQuery query);
    Task<PagedResult<PictureDto>> GetPictures(Query query);
    Task<PagedResult<PicturePreviewDto>> GetLikedPictures(int id, Query query);
    Task<PagedResult<PicturePreviewDto>> GetPostedPictures(int id, Query query);
    Task<PagedResult<PictureDto>> SearchAll(CustomQuery query);
    Task<IEnumerable<LikeDto>> GetPicLikes(int id);
    Task<IEnumerable<AccountDto>> GetPicLikers(int id);
    Task<PictureDto> GetById(int id);
    Task<string> Create(CreatePictureDto dto);
    // Task<PictureDto> Update(int id, UpdatePictureDto dto);
    Task<bool> Delete(int id);
}