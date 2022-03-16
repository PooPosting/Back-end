using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Profilers;

public class PictureMappingProfile : Profile
{
    public PictureMappingProfile()
    {
        CreateMap<Picture, PictureDto>()
            .ForMember(
                pto => pto.Tags,
                opt => opt.MapFrom(
                    p => SerializeTags(p.Tags)))
            .ForMember(
                dto => dto.AccountNickname,
                opt => opt.MapFrom(
                    p => p.Account.IsDeleted ? "Unknown" : p.Account.Nickname))
            .ForMember(
                dto => dto.AccountId,
                opt => opt.MapFrom(
                    p => p.Account.IsDeleted ? Guid.Empty : p.Account.Id));

        CreateMap<Account, AccountDto>()
            .ForMember(dto => dto.Email,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? string.Empty : a.Email))
            .ForMember(dto => dto.Nickname,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? "Unknown" : a.Nickname))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? Guid.Empty : a.Id));
        
        CreateMap<Comment, CommentDto>()
            .ForMember(dto => dto.AuthorNickname,
                opt => opt.MapFrom(
                    c => c.Author.Nickname))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    c => c.Picture.Id))
            .ForMember(dto => dto.AuthorId,
                opt => opt.MapFrom(
                    c => c.Author.Id));
        
        CreateMap<CreateAccountDto, Account>()
            .ForMember(
                acc => acc.AccountCreated,
                opt => opt.MapFrom(
                    c => DateTime.Now));

        CreateMap<CreatePictureDto, Picture>()
            .ForMember(
                pic => pic.Tags,
                opt => opt
                    .MapFrom(c => string.Join(" ", c.Tags).ToLower()));

        CreateMap<Like, LikeDto>()
            .ForMember(dto => dto.AccountNickname,
                opt => opt.MapFrom(
                    l => l.Liker.Nickname))
            .ForMember(dto => dto.AccountId,
                opt => opt.MapFrom(
                    l => l.Liker.Id))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    l => l.Liked.Id));

        
    }

    private static List<string> SerializeTags(string tags)
    {
        var tagList = tags.Split(' ').ToList();
        return tagList;
    }
}