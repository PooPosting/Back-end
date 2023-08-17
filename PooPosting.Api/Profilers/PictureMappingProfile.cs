using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Profilers.ValueResolvers;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Profilers;

public class PictureMappingProfile : Profile
{
    public PictureMappingProfile()
    {
        CreateMap<Picture, PictureDto>()
            .ForMember(dto => dto.LikeState, opt => opt.MapFrom<LikeStateResolver>())
            .ForMember(dto => dto.Comment, opt => opt.MapFrom(p => p.Comments.FirstOrDefault()))
            .ForMember(dto => dto.Url, opt => opt.MapFrom<UrlResolver>())
            .ForMember(dto => dto.Tags, opt => opt.MapFrom(p => p.PictureTags.Select(pt => pt.Tag.Value)))
            .ForMember(dto => dto.Id, opt => opt.MapFrom(p => IdHasher.EncodePictureId(p.Id)))
            .ForMember(dto => dto.LikeCount, opt => opt.MapFrom(p => p.Likes.Count(l => l.IsLike)))
            .ForMember(dto => dto.CommentCount, opt => opt.MapFrom(p => p.Comments.Count()));

        CreateMap<PictureDto, Picture>()
            .ForMember(p => p.Id, opt => opt.MapFrom(pto => IdHasher.DecodePictureId(pto.Id)));

        CreateMap<CreatePictureDto, Picture>();
    }

}