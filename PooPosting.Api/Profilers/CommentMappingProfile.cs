using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Profilers;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(c => IdHasher.EncodeCommentId(c.Id)))
            .ForMember(dto => dto.PictureId, opt => opt.MapFrom(c => IdHasher.EncodePictureId(c.PictureId)));
        
        CreateMap<CommentDto, Comment>()
            .ForMember(c => c.Id, opt => opt.MapFrom(dto => IdHasher.DecodeCommentId(dto.Id)))
            .ForMember(c => c.PictureId, opt => opt.MapFrom(dto => IdHasher.DecodePictureId(dto.PictureId)));
    }
}