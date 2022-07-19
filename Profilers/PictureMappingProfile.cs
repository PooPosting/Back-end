using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Picture;
using PicturesAPI.Profilers.ValueResolvers;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class PictureMappingProfile : Profile
{
    public PictureMappingProfile()
    {
        CreateMap<Picture, PictureDto>()
            .ForMember(dto => dto.LikeState,
                opt => opt.MapFrom<LikeStateResolver>())
            .ForMember(dto => dto.AccountPreview,
                opt => opt.MapFrom(
                    p => p.Account))
            .ForMember(dto => dto.Url,
                opt => opt.MapFrom<UrlResolver>())
            .ForMember(dto => dto.IsModifiable,
                opt => opt.MapFrom<ModifiableResolver>())
            .ForMember(dto => dto.IsAdminModifiable,
                opt => opt.MapFrom<AdminModifiableResolver>())
            .ForMember(dto => dto.Tags,
                opt => opt.MapFrom(
                    p => p.PictureTags.Select(pt => pt.Tag.Value)))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    p => IdHasher.EncodePictureId(p.Id)))
            .ForMember(dto => dto.LikeCount,
                opt => opt.MapFrom(
                    p => p.Likes.Count(l => l.IsLike)))
            .ForMember(dto => dto.DislikeCount,
                opt => opt.MapFrom(
                    p => p.Likes.Count(l => !l.IsLike)))
            .ForMember(dto => dto.CommentCount,
                opt => opt.MapFrom(
                    p => p.Comments.Count()));

        CreateMap<Picture, PicturePreviewDto>()
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    p => IdHasher.EncodePictureId(p.Id)))
            .ForMember(dto => dto.Url,
                opt => opt.MapFrom<UrlResolver>());

        CreateMap<PictureDto, Picture>()
            .ForMember(p => p.Id,
                opt => opt.MapFrom(
                    pto => IdHasher.DecodePictureId(pto.Id)));

        CreateMap<CreatePictureDto, Picture>();
    }

}