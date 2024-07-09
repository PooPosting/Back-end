using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Dtos.Comment;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Mappers;

public static class AccountMapper
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
    
    public static IQueryable<AccountDto> ProjectToDto(this IQueryable<Account> queryable)
    {
        return queryable
            .Select(a => new AccountDto
                {
                    Id = IdHasher.EncodeAccountId(a.Id),
                    Nickname = a.Nickname,
                    Email = a.Email,
                    ProfilePicUrl = a.ProfilePicUrl,
                    RoleId = a.RoleId,
                    AccountCreated = a.AccountCreated,
                    Pictures = a.Pictures.Select(p => new PictureDto
                    {
                        Id = IdHasher.EncodePictureId(p.Id),
                        Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
                        Description = p.Description,
                        Account = new AccountDto
                        {
                            Id = IdHasher.EncodeAccountId(p.Account.Id),
                        },
                        Comments = p.Comments
                            .OrderByDescending(c => c.CommentAdded)
                            .Select(c => new CommentDto
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
                            })
                            .Take(3),
                        Url = p.Url,
                        PictureAdded = p.PictureAdded,
                        LikeCount = p.Likes.Count, 
                        CommentCount = p.Comments.Count, 
                        IsLiked = CurrAccId() != null && p.Likes.Any(l => l.AccountId == CurrAccId())
                    }).Take(4),
                    PictureCount = a.Pictures.Count,
                    LikeCount = a.Pictures.Sum(p => p.Likes.Count),
                    CommentCount = a.Pictures.Sum(p => p.Comments.Count)
                }
            );
    }
    
    public static AccountDto MapToDto(this Account a)
    {
        var dto = new AccountDto
        {
            Id = IdHasher.EncodeAccountId(a.Id),
            Nickname = a.Nickname,
            Email = a.Email,
            ProfilePicUrl = a.ProfilePicUrl,
            RoleId = a.RoleId,
            AccountCreated = a.AccountCreated,
        };

        return dto;
    }
}