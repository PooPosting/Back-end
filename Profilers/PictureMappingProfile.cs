using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Profilers;

public class PictureMappingProfile : Profile
{
    public PictureMappingProfile()
    {

        CreateMap<Picture, PictureDto>()
            .ForMember(
                p => p.Tags,
                c => c
                    .MapFrom(s => SerializeTags(s.Tags) ))
            .ForMember(
                p => p.Likes,
                m => m
                    .MapFrom(p => p.Likes.Count(l => l.IsLike == true)))
            .ForMember(
                p => p.Dislikes,
                m => m
                    .MapFrom(p => p.Likes.Count(l => l.IsLike == false)))
            .ForMember(
                d => d.AccountNickname,
                m => m.MapFrom(
                    p => p.Account.IsDeleted ? "Unknown" : p.Account.Nickname))
            .ForMember(
                d => d.AccountId,
                m => m.MapFrom(
                    p => p.Account.IsDeleted ? Guid.Empty : p.Account.Id));

        CreateMap<Account, AccountDto>()
            .ForMember(d => d.Email,
                m => m.MapFrom(
                    a => a.IsDeleted ? string.Empty : a.Email))
            .ForMember(d => d.Nickname,
                m => m.MapFrom(
                    a => a.IsDeleted ? "Unknown" : a.Nickname))
            .ForMember(d => d.Id,
                m => m.MapFrom(
                    a => a.IsDeleted ? Guid.Empty : a.Id));

        CreateMap<CreateAccountDto, Account>()
            .ForMember(
                c => c.AccountCreated,
                a => a.MapFrom(
                    m => DateTime.Now));

        CreateMap<CreatePictureDto, Picture>()
            .ForMember(
                p => p.Tags,
                p => p
                    .MapFrom(c => string.Join(" ", c.Tags).ToLower()));
    }

    private static List<string> SerializeTags(string tags)
    {
        var tagList = tags.Split(' ').ToList();
        return tagList;
    }
}