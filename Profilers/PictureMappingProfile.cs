using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class PictureMappingProfile : Profile
{
    public PictureMappingProfile()
    {
        CreateMap<Picture, PictureDto>()
            .ForMember(dto => dto.Tags,
                opt => opt.MapFrom(
                    p => p.PictureTagJoins.Select(p => p.Tag.Value)))
            .ForMember(
                dto => dto.AccountNickname,
                opt => opt.MapFrom(
                    p => p.Account.IsDeleted ? "Unknown" : p.Account.Nickname))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    p => IdHasher.EncodePictureId(p.Id)))
            .ForMember(
                dto => dto.AccountId,
                opt => opt.MapFrom(
                    p => p.Account.IsDeleted ? "0" : IdHasher.EncodeAccountId(p.Account.Id)))
            .ForMember(dto => dto.LikeCount,
                opt => opt.MapFrom(
                    p => p.Likes.Count(l => l.IsLike)))
            .ForMember(dto => dto.DislikeCount,
                opt => opt.MapFrom(
                    p => p.Likes.Count(l => !l.IsLike)))
            .ForMember(dto => dto.CommentCount,
                opt => opt.MapFrom(
                    p => p.Comments.Count));

        CreateMap<PictureDto, Picture>()
            .ForMember(p => p.Id,
                opt => opt.MapFrom(
                    pto => IdHasher.DecodePictureId(pto.Id)))
            .ForMember(p => p.AccountId,
                opt => opt.MapFrom(
                    dto => IdHasher.DecodeAccountId(dto.AccountId)));

        CreateMap<CreatePictureDto, Picture>();
    }

}