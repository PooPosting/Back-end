using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dto => dto.AuthorNickname,
                opt => opt.MapFrom(
                    c => c.Author.Nickname))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    c => GuidEncoder.Encode(c.Picture.Id)))
            .ForMember(dto => dto.AuthorId,
                opt => opt.MapFrom(
                    c => GuidEncoder.Encode(c.Author.Id)))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    c => GuidEncoder.Encode(c.Id)));
        
        CreateMap<CommentDto, Comment>()
            .ForMember(c => c.Id,
                opt => opt.MapFrom(
                    dto => GuidEncoder.Decode(dto.Id)));
    }
}