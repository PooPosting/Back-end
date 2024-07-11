using PooPosting.Application.Models.Dtos.Comment;
using PooPosting.Application.Models.Dtos.Comment.Out;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Mappers;

public static class CommentMapper
{
    public static IQueryable<CommentDto> ProjectToDto(this IQueryable<Comment> queryable) 
        => queryable.Select(c => c.MapToDto());
    
    public static CommentDto MapToDto(this Comment c)
    {
        var dto = new CommentDto()
        {
            Id = IdHasher.EncodeCommentId(c.Id),
            Text = c.Text,
            CommentAdded = c.CommentAdded,
            PictureId = IdHasher.EncodePictureId(c.PictureId),
            Account = c.Account.MapToDto()
        };

        return dto;
    }
}