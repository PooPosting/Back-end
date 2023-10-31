using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Mappers;

public static class PictureMapper
{
    public static IQueryable<PictureDto> ProjectToDto(this IQueryable<Picture> queryable, int? currAccId)
    {
        return queryable
            .Select(p => new PictureDto
                {
                    Id = IdHasher.EncodePictureId(p.Id),
                    Tags = p.PictureTags.Select(t => t.Tag.Value).ToList(),
                    Name = p.Name,
                    Description = p.Description,
                    Account = new AccountDto
                    {
                        Id = IdHasher.EncodeAccountId(p.Account.Id),
                        Nickname = p.Account.Nickname,
                        Email = p.Account.Email,
                        ProfilePicUrl = p.Account.ProfilePicUrl,
                        BackgroundPicUrl = p.Account.BackgroundPicUrl,
                        AccountDescription = p.Account.AccountDescription,
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
                    IsLiked = currAccId != null && p.Likes != null && p.Likes.Any(l => l.AccountId == currAccId)
                }
            );
    }
    
    public static PictureDto MapToDto(this Picture p, int? accountId)
    {
        if (p == null) return null;

        var pictureDto = new PictureDto()
        {
            Id = IdHasher.EncodePictureId(p.Id),
            Tags = p.PictureTags?.Select(t => t.Tag.Value).ToList(),
            Name = p.Name,
            Description = p.Description,
            Account = p.Account != null ? new AccountDto
            {
                Id = IdHasher.EncodeAccountId(p.Account.Id),
                Nickname = p.Account.Nickname,
                Email = p.Account.Email,
                ProfilePicUrl = p.Account.ProfilePicUrl,
                BackgroundPicUrl = p.Account.BackgroundPicUrl,
                AccountDescription = p.Account.AccountDescription,
                RoleId = p.Account.RoleId,
                AccountCreated = p.Account.AccountCreated
            } : null,
            Comments = p.Comments?.Select(c => new CommentDto
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
            }).ToList(),
            Url = p.Url,
            PictureAdded = p.PictureAdded,
            LikeCount = p.Likes?.Count ?? 0, 
            CommentCount = p.Comments?.Count ?? 0, 
            IsLiked = accountId != null && p.Likes != null && p.Likes.Any(l => l.AccountId == accountId)
        };

        return pictureDto;
    }
}