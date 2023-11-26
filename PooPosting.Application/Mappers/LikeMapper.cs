using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Mappers;

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
                        RoleId = l.Account.RoleId,
                        AccountCreated = l.Account.AccountCreated
                    }
                }
            );
    }
    
    public static LikeDto MapToDto(this Like l)
    {
        var dto = new LikeDto
        {
            Id = l.Id,
            PictureId = IdHasher.EncodePictureId(l.PictureId),
            Account = new AccountDto
            {
                Id = IdHasher.EncodeAccountId(l.Account.Id),
                Nickname = l.Account.Nickname,
                Email = l.Account.Email,
                ProfilePicUrl = l.Account.ProfilePicUrl,
                RoleId = l.Account.RoleId,
                AccountCreated = l.Account.AccountCreated
            }
        };

        return dto;
    }
}