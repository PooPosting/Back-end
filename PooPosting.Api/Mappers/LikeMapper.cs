using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Mappers;

public static class LikeMapper
{
    public static IQueryable<LikeDto> ProjectToDto(this IQueryable<Like> queryable)
    {
        return queryable
            .Select(l => new LikeDto
                {
                    Id = l.Id,
                    PictureId = IdHasher.EncodePictureId(l.PictureId),
                    Account = new AccountDto
                    {
                        Id = IdHasher.EncodeAccountId(l.Account.Id),
                        Nickname = l.Account.Nickname,
                        Email = l.Account.Email,
                        ProfilePicUrl = l.Account.ProfilePicUrl,
                        BackgroundPicUrl = l.Account.BackgroundPicUrl,
                        AccountDescription = l.Account.AccountDescription,
                        RoleId = l.Account.RoleId,
                        AccountCreated = l.Account.AccountCreated
                    }
                }
            );
    }
    
    public static LikeDto MapToDto(this Like l)
    {
        if (l == null) return null;

        var dto = new LikeDto
        {
            Id = l.Id,
            PictureId = IdHasher.EncodePictureId(l.PictureId),
            Account = l.Account is null ? null : new AccountDto
            {
                Id = IdHasher.EncodeAccountId(l.Account.Id),
                Nickname = l.Account.Nickname,
                Email = l.Account.Email,
                ProfilePicUrl = l.Account.ProfilePicUrl,
                BackgroundPicUrl = l.Account.BackgroundPicUrl,
                AccountDescription = l.Account.AccountDescription,
                RoleId = l.Account.RoleId,
                AccountCreated = l.Account.AccountCreated
            }
        };

        return dto;
    }
}