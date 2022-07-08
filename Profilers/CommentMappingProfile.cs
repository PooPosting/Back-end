using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Profilers.ValueResolvers;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dto => dto.AccountPreview,
                opt => opt.MapFrom(
                    c => c.Account))
            .ForMember(dto => dto.IsModifiable,
                opt => opt.MapFrom<ModifiableResolver>())
            .ForMember(dto => dto.IsAdminModifiable,
                opt => opt.MapFrom<AdminModifiableResolver>())
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    c => IdHasher.EncodePictureId(c.Picture.Id)))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    c => IdHasher.EncodeCommentId(c.Id)));
        
        CreateMap<CommentDto, Comment>()
            .ForMember(c => c.Id,
                opt => opt.MapFrom(
                    dto => IdHasher.DecodeCommentId(dto.Id)));
    }
}