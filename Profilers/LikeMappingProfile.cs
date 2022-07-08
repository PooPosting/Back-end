using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class LikeMappingProfile : Profile
{
    public LikeMappingProfile()
    {
        CreateMap<Like, LikeDto>()
            .ForMember(dto => dto.AccountPreview,
                opt => opt.MapFrom(
                    l => l.Account))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    l => IdHasher.EncodePictureId(l.Picture.Id)));
    }
}