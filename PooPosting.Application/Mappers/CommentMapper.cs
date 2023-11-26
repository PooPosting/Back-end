using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Mappers;

public static class CommentMapper
{
    public static IQueryable<CommentDto> ProjectToDto(this IQueryable<Comment> queryable)
    {
        return queryable
            .Select(c => new CommentDto()
                {
                    Id = IdHasher.EncodeCommentId(c.Id),
                    Text = c.Text,
                    CommentAdded = c.CommentAdded,
                    PictureId = IdHasher.EncodePictureId(c.PictureId),
                    Account = new AccountDto
                    {
                        Id = IdHasher.EncodeAccountId(c.Account.Id),
                        Nickname = c.Account.Nickname,
                        Email = c.Account.Email,
                        ProfilePicUrl = c.Account.ProfilePicUrl,
                        RoleId = c.Account.RoleId,
                        AccountCreated = c.Account.AccountCreated
                    }
                }
            );
    }
    
    public static CommentDto MapToDto(this Comment c)
    {
        var dto = new CommentDto()
        {
            Id = IdHasher.EncodeCommentId(c.Id),
            Text = c.Text,
            CommentAdded = c.CommentAdded,
            PictureId = IdHasher.EncodePictureId(c.PictureId),
            Account = new AccountDto
            {
                Id = IdHasher.EncodeAccountId(c.Account.Id),
                Nickname = c.Account.Nickname,
                Email = c.Account.Email,
                ProfilePicUrl = c.Account.ProfilePicUrl,
                RoleId = c.Account.RoleId,
                AccountCreated = c.Account.AccountCreated
            }
        };

        return dto;
    }
}