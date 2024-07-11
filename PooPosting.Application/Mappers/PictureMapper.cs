using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Models.Dtos.Picture.Out;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Mappers;

public static class PictureMapper
{
    public static IQueryable<PictureDto> ProjectToDto(this IQueryable<Picture> queryable)
    {
        return queryable.Select(p => p.MapToDto());
    }
    
    public static PictureDto MapToDto(this Picture p)
    {
        return new PictureDto
        {
            Id = IdHasher.EncodePictureId(p.Id),
            Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
            Description = p.Description,
            Account = p.Account.MapToDto(),
            Comments = p.Comments
                .OrderByDescending(c => c.CommentAdded)
                .Take(3)
                .Select(c => c.MapToDto())
                .ToList(),
            Url = p.Url,
            PictureAdded = p.PictureAdded,
            LikeCount = p.Likes.Count, 
            CommentCount = p.Comments.Count, 
            IsLiked = MapperContext.CurrentUserId != null && p.Likes.Any(l => l.AccountId == MapperContext.CurrentUserId)
        };
    }
}