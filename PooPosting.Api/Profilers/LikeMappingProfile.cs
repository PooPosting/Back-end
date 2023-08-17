using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Profilers;

public class LikeMappingProfile : Profile
{
    public LikeMappingProfile()
    {
        CreateMap<Like, LikeDto>()
            .ForMember(dto => dto.PictureId, opt => opt.MapFrom(l => IdHasher.EncodePictureId(l.Picture.Id)));
    }
}