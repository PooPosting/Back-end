﻿using System.Security.Claims;
using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Entities.Interfaces;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Profilers.ValueResolvers;

public class ModifiableResolver:
    IValueResolver<Account, AccountDto, bool>,
    IValueResolver<Picture, PictureDto, bool>,
    IValueResolver<Comment, CommentDto, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ModifiableResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool Resolve(IModifiable source, object destination, bool destMember, ResolutionContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is not null)
        {
            var accId = httpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (accId is not null)
            {
                return accId.Value == source.AccountId.ToString();
            }
        }
        return false;
    }

    public bool Resolve(Account source, AccountDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var accId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (accId is not null)
        {
            return accId.Value == source.Id.ToString();
        }
        return false;
    }

    public bool Resolve(Picture source, PictureDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var accId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (accId is not null)
        {
            return accId.Value == source.AccountId.ToString();
        }
        return false;
    }

    public bool Resolve(Comment source, CommentDto destination, bool destMember, ResolutionContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var accId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (accId is not null)
        {
            return accId.Value == source.AccountId.ToString();
        }
        return false;
    }
}