using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Profilers;

public class AccountMappingProfile: Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(dto => dto.Email,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? string.Empty : a.Email))
            .ForMember(dto => dto.Nickname,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? "Unknown" : a.Nickname))
            .ForMember(dto => dto.Id,
                opt => opt.MapFrom(
                    a => a.IsDeleted ? "0" : IdHasher.EncodeAccountId(a.Id)))
            .ForMember(dto => dto.Pictures,
                opt => opt.MapFrom(
                    acc => acc.Pictures.Where(p => !p.IsDeleted)))
            .ForMember(dto => dto.RoleId,
                opt => opt.MapFrom(
                    acc => acc.Role.Id));

        CreateMap<AccountDto, Account>()
            .ForMember(acc => acc.Id,
                opt => opt.MapFrom(
                    ato => IdHasher.DecodeAccountId(ato.Id)));

        CreateMap<CreateAccountDto, Account>()
            .ForMember(
                acc => acc.AccountCreated,
                opt => opt.MapFrom(
                    c => DateTime.Now));
    }
}