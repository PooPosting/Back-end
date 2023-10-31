using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Mappers;

public static class AccountMapper
{
    public static IQueryable<AccountDto> ProjectToDto(this IQueryable<Account> queryable, int? currAccId)
    {
        return queryable
            .Select(a => new AccountDto
                {
                    Id = IdHasher.EncodeAccountId(a.Id),
                    Nickname = a.Nickname,
                    Email = a.Email,
                    ProfilePicUrl = a.ProfilePicUrl,
                    BackgroundPicUrl = a.BackgroundPicUrl,
                    AccountDescription = a.AccountDescription,
                    RoleId = a.RoleId,
                    AccountCreated = a.AccountCreated,
                    Pictures = a.Pictures.Select(p => new PictureDto
                    {
                        Id = IdHasher.EncodePictureId(p.Id),
                        Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
                        Name = p.Name,
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
                                Account = c.Account != null ? new AccountDto()
                                {
                                    Id = IdHasher.EncodeAccountId(c.Account.Id),
                                    Nickname = c.Account.Nickname,
                                    Email = c.Account.Email,
                                    ProfilePicUrl = c.Account.ProfilePicUrl,
                                    BackgroundPicUrl = c.Account.BackgroundPicUrl,
                                    AccountDescription = c.Account.AccountDescription,
                                    RoleId = c.Account.RoleId,
                                    AccountCreated = c.Account.AccountCreated
                                } : null
                            })
                            .Take(3),
                        Url = p.Url,
                        PictureAdded = p.PictureAdded,
                        LikeCount = p.Likes.Count, 
                        CommentCount = p.Comments.Count, 
                        IsLiked = currAccId != null && p.Likes != null && p.Likes.Any(l => l.AccountId == currAccId)
                    }).Take(4),
                    PictureCount = a.Pictures.Count,
                    LikeCount = a.Pictures.Sum(p => p.Likes.Count),
                    CommentCount = a.Pictures.Sum(p => p.Comments.Count)
                }
            );
    }
    
    public static AccountDto MapToDto(this Account a, int? currAccId)
    {
        if (a == null) return null;

        var dto = new AccountDto
        {
            Id = IdHasher.EncodeAccountId(a.Id),
            Nickname = a.Nickname,
            Email = a.Email,
            ProfilePicUrl = a.ProfilePicUrl,
            BackgroundPicUrl = a.BackgroundPicUrl,
            AccountDescription = a.AccountDescription,
            RoleId = a.RoleId,
            AccountCreated = a.AccountCreated,
            Pictures = a.Pictures.Select(p => new PictureDto
            {
                Id = IdHasher.EncodePictureId(p.Id),
                Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
                Name = p.Name,
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
                        Account = c.Account is null ? null :  new AccountDto()
                        {
                            Id = IdHasher.EncodeAccountId(c.Account.Id),
                            Nickname = c.Account.Nickname,
                            Email = c.Account.Email,
                            ProfilePicUrl = c.Account.ProfilePicUrl,
                            BackgroundPicUrl = c.Account.BackgroundPicUrl,
                            AccountDescription = c.Account.AccountDescription,
                            RoleId = c.Account.RoleId,
                            AccountCreated = c.Account.AccountCreated
                        }
                    })
                    .Take(3),
                Url = p.Url,
                PictureAdded = p.PictureAdded,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count,
                IsLiked = currAccId != null && p.Likes != null && p.Likes.Any(l => l.AccountId == currAccId),
            }),
        };

        return dto;
    }
}