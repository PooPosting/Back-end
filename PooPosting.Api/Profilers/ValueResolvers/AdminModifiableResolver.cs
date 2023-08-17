using System.Security.Claims;
using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Profilers.ValueResolvers;

public class AdminModifiableResolver :
    IValueResolver<Picture, PictureDto, bool>,
    IValueResolver<Comment, CommentDto, bool>,
    IValueResolver<Account, AccountDto, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminModifiableResolver(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool Resolve(Picture source, PictureDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var roleId = user.FindFirst(c => c.Type == ClaimTypes.Role);
        if (roleId is not null) return roleId.Value == "3";
        return false;
    }

    public bool Resolve(Comment source, CommentDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var roleId = user.FindFirst(c => c.Type == ClaimTypes.Role);
        if (roleId is not null) return roleId.Value == "3";
        return false;
    }

    public bool Resolve(Account source, AccountDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var roleId = user.FindFirst(c => c.Type == ClaimTypes.Role);
        if (roleId is not null) return roleId.Value == "3";
        return false;
    }
}