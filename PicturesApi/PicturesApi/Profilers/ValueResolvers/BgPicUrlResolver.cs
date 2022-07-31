using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Account;

namespace PicturesAPI.Profilers.ValueResolvers;

public class BgPicUrlResolver:
    IValueResolver<Account, AccountDto, string>
{
    private readonly string _appOrigin;
    public BgPicUrlResolver(IConfiguration configuration)
    {
        _appOrigin = configuration.GetValue<string>("AppSettings:Origin");
    }

    public string Resolve(Account source, AccountDto destination, string destMember, ResolutionContext context)
    {
        return source.BackgroundPicUrl.StartsWith("http") ? source.BackgroundPicUrl : Path.Combine(_appOrigin, source.BackgroundPicUrl);
    }

}