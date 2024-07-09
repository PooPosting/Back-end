using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Dtos.Comment;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Mappers;

public static class PictureMapper
{
    private static IHttpContextAccessor? httpCtx;
    private static int? CurrAccId()
    {
        var userIdClaim = httpCtx?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var accountId) ? accountId : null;
    }
    public static void Init(IHttpContextAccessor httpContextAccessor)
    {
        httpCtx = httpContextAccessor;
    }
    
    public static IQueryable<PictureDto> ProjectToDto(this IQueryable<Picture> queryable)
    {
        return queryable
            .Select(p => new PictureDto
                {
                    Id = IdHasher.EncodePictureId(p.Id),
                    Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
                    Description = p.Description,
                    Account = new AccountDto
                    {
                        Id = IdHasher.EncodeAccountId(p.Account.Id),
                        Nickname = p.Account.Nickname,
                        Email = p.Account.Email,
                        ProfilePicUrl = p.Account.ProfilePicUrl,
                        RoleId = p.Account.RoleId,
                        AccountCreated = p.Account.AccountCreated
                    },
                    Comments = p.Comments
                        .OrderByDescending(c => c.CommentAdded)
                        .Select(c => new CommentDto
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
                        })
                        .Take(3),
                    Url = p.Url,
                    PictureAdded = p.PictureAdded,
                    LikeCount = p.Likes.Count, 
                    CommentCount = p.Comments.Count, 
                    IsLiked = CurrAccId() != null && p.Likes.Any(l => l.AccountId == CurrAccId())
                }
            );
    }
    
    public static PictureDto MapToDto(this Picture p)
    {
        var pictureDto = new PictureDto()
        {
            Id = IdHasher.EncodePictureId(p.Id),
            Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
            Description = p.Description,
            Account = new AccountDto
            {
                Id = IdHasher.EncodeAccountId(p.Account.Id),
                Nickname = p.Account.Nickname,
                Email = p.Account.Email,
                ProfilePicUrl = p.Account.ProfilePicUrl,
                RoleId = p.Account.RoleId,
                AccountCreated = p.Account.AccountCreated
            },
            Comments = p.Comments.Select(c => new CommentDto
            {
                Id = IdHasher.EncodeCommentId(c.Id),
                Text = c.Text,
                CommentAdded = c.CommentAdded,
                PictureId = IdHasher.EncodePictureId(c.PictureId),
                Account = new AccountDto()
                {
                    Id = IdHasher.EncodeAccountId(c.Account.Id),
                    Nickname = c.Account.Nickname,
                    Email = c.Account.Email,
                    ProfilePicUrl = c.Account.ProfilePicUrl,
                    RoleId = c.Account.RoleId,
                    AccountCreated = c.Account.AccountCreated
                }
            }).ToList(),
            Url = p.Url,
            PictureAdded = p.PictureAdded,
            LikeCount = p.Likes.Count, 
            CommentCount = p.Comments.Count, 
            IsLiked = CurrAccId() != null && p.Likes.Any(l => l.AccountId == CurrAccId())
        };

        return pictureDto;
    }
}