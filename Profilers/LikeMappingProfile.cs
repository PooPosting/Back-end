using AutoMapper;
using PicturesAPI.Configuration;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class LikeMappingProfile : Profile
{
    public LikeMappingProfile()
    {
        CreateMap<Like, LikeDto>()
            .ForMember(dto => dto.AccountNickname,
                opt => opt.MapFrom(
                    l => l.Liker.Nickname))
            .ForMember(dto => dto.AccountId,
                opt => opt.MapFrom(
                    l => IdHasher.EncodeAccountId(l.Liker.Id)))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    l => IdHasher.EncodePictureId(l.Liked.Id)));
    }
}