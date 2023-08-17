using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Profilers.ValueResolvers;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Profilers;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(c => IdHasher.EncodeCommentId(c.Id)));
        
        CreateMap<CommentDto, Comment>()
            .ForMember(c => c.Id, opt => opt.MapFrom(dto => IdHasher.DecodeCommentId(dto.Id)));
    }
}