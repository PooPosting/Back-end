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
                    c => c.Account.Nickname))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    c => IdHasher.EncodePictureId(c.Picture.Id)))
            .ForMember(dto => dto.AccountId,
                opt => opt.MapFrom(
                    c => IdHasher.EncodeAccountId(c.Account.Id)))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    c => IdHasher.EncodeCommentId(c.Id)));
        
        CreateMap<CommentDto, Comment>()
            .ForMember(c => c.Id,
                opt => opt.MapFrom(
                    dto => IdHasher.DecodeCommentId(dto.Id)));
    }
}