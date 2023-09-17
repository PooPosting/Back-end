using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Profilers;

public class PictureMappingProfile : Profile
{
    public PictureMappingProfile()
    {
        CreateMap<Picture, PictureDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(p => IdHasher.EncodePictureId(p.Id)))
            .ForMember(dto => dto.Comments, opt => opt.MapFrom(p => p.Comments.OrderByDescending(c => c.CommentAdded).Take(5)))
            .ForMember(dto => dto.Tags, opt => opt.MapFrom(p => p.PictureTags.Select(pt => pt.Tag.Value)))
            .ForMember(dto => dto.LikeCount, opt => opt.MapFrom(p => p.Likes.Count(l => l.IsLike)))
            .ForMember(dto => dto.CommentCount, opt => opt.MapFrom(p => p.Comments.Count));
        
        CreateMap<PictureDto, Picture>()
            .ForMember(p => p.Id, opt => opt.MapFrom(pto => IdHasher.DecodePictureId(pto.Id)));

        CreateMap<CreatePictureDto, Picture>();
    }

}