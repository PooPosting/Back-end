using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PooPosting.Data.DbContext.Entities;
using PooPosting.Service.Models.Dtos.Account;
using PooPosting.Service.Models.Dtos.Comment;
using PooPosting.Service.Services.Helpers;

namespace PooPosting.Service.Mappers;

public static class CommentMapper
{
    private static IHttpContextAccessor httpCtx = null!;
    private static int? CurrAccId => int.Parse(httpCtx.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
    
    public static void Init(IHttpContextAccessor httpContextAccessor)
    {
        httpCtx = httpContextAccessor;
    }
    
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