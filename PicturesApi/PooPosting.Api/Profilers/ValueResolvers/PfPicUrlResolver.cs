using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Profilers.ValueResolvers;

public class PfPicUrlResolver:
    IValueResolver<Account, AccountDto, string>,
    IValueResolver<Account, AccountPreviewDto, string>
{
    private readonly string _appOrigin;
    public PfPicUrlResolver(IConfiguration configuration)
    {
        _appOrigin = configuration.GetValue<string>("AppSettings:Origin");
    }

    public string Resolve(Account source, AccountDto destination, string destMember, ResolutionContext context)
    {
        if (source.ProfilePicUrl is null) return null;
        return source.ProfilePicUrl.StartsWith("http") ? source.ProfilePicUrl : Path.Combine(_appOrigin, source.ProfilePicUrl);
    }

    public string Resolve(Account source, AccountPreviewDto destination, string destMember, ResolutionContext context)
    {
        if (source.ProfilePicUrl is null) return null;
        return source.ProfilePicUrl.StartsWith("http") ? source.ProfilePicUrl : Path.Combine(_appOrigin, source.ProfilePicUrl);
    }
}