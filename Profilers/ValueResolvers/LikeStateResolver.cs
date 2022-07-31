using System.Security.Claims;
using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Picture;

namespace PicturesAPI.Profilers.ValueResolvers;

public class LikeStateResolver : IValueResolver<Picture, PictureDto, LikeState>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeStateResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public LikeState Resolve(Picture source, PictureDto destination, LikeState destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var accId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (accId is not null)
        {
            var like = source.Likes.SingleOrDefault(l => l.AccountId.ToString() == accId.Value);

            if (like is not null)
            {
                return like.IsLike ? LikeState.Liked : LikeState.DisLiked;
            }
        }
        return LikeState.Deleted;
    }
}