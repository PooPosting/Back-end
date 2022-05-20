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

        CreateMap<CreatePictureDto, Picture>()
            .ForMember(
                pic => pic.Tags,
                opt => opt
                    .MapFrom(c => string.Join(" ", c.Tags).ToLower()));
    }

    private static List<string> SerializeTags(string tags)
    {
        var tagList = tags.Split(' ').ToList();
        return tagList;
    }


}