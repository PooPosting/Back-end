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
                    .MapFrom(s => GetTags(s.Tags) ))
            .ForMember(
                p => p.Likes,
                m => m
                    .MapFrom(p => p.Likes.Count(l => l.IsLike == true)))
            .ForMember(
                p => p.Dislikes,
                m => m
                    .MapFrom(p => p.Likes.Count(l => l.IsLike == false)));

        CreateMap<Account, AccountDto>();

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

    private static List<string> GetTags(string tags)
    {
        var tagList = tags.Split(' ').ToList();
        return tagList;
    }
}