using System.Security.Claims;
using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Profilers.ValueResolvers;

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
        if (roleId is not null)
        {
            return roleId.Value == "3";
        }
        return false;
    }

    public bool Resolve(Comment source, CommentDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var roleId = user.FindFirst(c => c.Type == ClaimTypes.Role);
        if (roleId is not null)
        {
            return roleId.Value == "3";
        }
        return false;
    }

    public bool Resolve(Account source, AccountDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var roleId = user.FindFirst(c => c.Type == ClaimTypes.Role);
        if (roleId is not null)
        {
            return roleId.Value == "3";
        }
        return false;
    }
}