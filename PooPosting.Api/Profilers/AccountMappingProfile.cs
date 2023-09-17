using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Profilers;

public class AccountMappingProfile: Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(a => IdHasher.EncodeAccountId(a.Id)));

        CreateMap<AccountDto, Account>()
            .ForMember(acc => acc.Id, opt => opt.MapFrom(ato => IdHasher.DecodeAccountId(ato.Id)));

        CreateMap<CreateAccountDto, Account>()
            .ForMember(acc => acc.AccountCreated, opt => opt.MapFrom(c => DateTime.Now));

    }
}