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
            .ForMember(
                pto => pto.Tags,
                opt => opt.MapFrom(
                    p => SerializeTags(p.Tags)))
            .ForMember(
                dto => dto.AccountNickname,
                opt => opt.MapFrom(
                    p => p.Account.IsDeleted ? "Unknown" : p.Account.Nickname))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    p => GuidEncoder.Encode(p.Id)))
            .ForMember(
                dto => dto.AccountId,
                opt => opt.MapFrom(
                    p => p.Account.IsDeleted ? Guid.Empty.ToString() : GuidEncoder.Encode(p.Account.Id)));

        CreateMap<PictureDto, Picture>()
            .ForMember(p => p.Id,
                opt => opt.MapFrom(
                    pto => GuidEncoder.Decode(pto.Id)))
            .ForMember(p => p.AccountId,
                opt => opt.MapFrom(
                    dto => GuidEncoder.Decode(dto.Id)));

        CreateMap<Account, AccountDto>()
            .ForMember(dto => dto.Email,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? string.Empty : a.Email))
            .ForMember(dto => dto.Nickname,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? "Unknown" : a.Nickname))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? Guid.Empty.ToString() : GuidEncoder.Encode(a.Id)));
        
        CreateMap<AccountDto, Account>()
            .ForMember(acc => acc.Id,
                opt => opt.MapFrom(
                    ato => GuidEncoder.Decode(ato.Id)));
        
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
                    l => GuidEncoder.Encode(l.Liker.Id)))
            .ForMember(dto => dto.PictureId,
                opt => opt.MapFrom(
                    l => GuidEncoder.Encode(l.Liked.Id)));
    }

    private static List<string> SerializeTags(string tags)
    {
        var tagList = tags.Split(' ').ToList();
        return tagList;
    }


}