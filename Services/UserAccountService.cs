using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class UserAccountService : IUserAccountService
{
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IAccountRepo _accountRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IMapper _mapper;
    private readonly IRoleRepo _roleRepo;
    private readonly string _jwtIssuer;

    public UserAccountService(
        IPasswordHasher<Account> passwordHasher, 
        AuthenticationSettings authenticationSettings,
        IAccountRepo accountRepo,
        ILikeRepo likeRepo,
        IMapper mapper,
        IConfiguration config,
        IRoleRepo roleRepo)
    {
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _accountRepo = accountRepo;
        _likeRepo = likeRepo;
        _mapper = mapper;
        _roleRepo = roleRepo;
        _jwtIssuer = config.GetValue<string>("Authentication:JwtIssuer");
    }
        
    public int Create(CreateAccountDto dto)
    {
        var newAccount = new Account()
        {
            Nickname = dto.Nickname,
            Email = dto.Email
        };

        var hashedPassword = _passwordHasher.HashPassword(newAccount, dto.Password);
        newAccount.PasswordHash = hashedPassword;
        
        var result = _accountRepo.Insert(newAccount);
        _accountRepo.Save();
        return result;
    }
    public LoginSuccessResult GenerateJwt(LoginDto dto)
    {
        var account = _accountRepo.GetByNick(dto.Nickname);
            if (account is null || account.IsDeleted)
                throw new BadRequestException("Invalid nickname or password");

            var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid nickname or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, (account.Id.ToString())),
                new Claim(ClaimTypes.Name, account.Nickname),
                new Claim(ClaimTypes.Role, account.Role.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            var loginSuccessResult = new LoginSuccessResult()
            {
                AccountDto = _mapper.Map<AccountDto>(account),
                AuthToken = tokenHandler.WriteToken(token),
                Likes = _mapper.Map<List<LikeDto>>(_likeRepo.GetByLikerId(account.Id)),
                // Verified = account.Verified
            };

            return loginSuccessResult;

    }

    //rewrite this whole method
    public LsLoginSuccessResult VerifyJwt(LsLoginDto dto)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = handler.ReadToken(dto.JwtToken) as JwtSecurityToken;
            if (jwtToken is null) throw new InvalidAuthTokenException();
        }
        catch (Exception)
        {
            throw new InvalidAuthTokenException();
        }
        if (jwtToken.Issuer != _jwtIssuer) throw new InvalidAuthTokenException();


        var id = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        if (
            (_accountRepo.GetById(int.Parse(id)) is null)
        )
        {
            throw new UnauthorizedException("please log in");
        }

        if (
            false
            // (DateTime.Now - DateTime.Parse(exp) > TimeSpan.FromDays(5))
        )
        {
            throw new ExpiredJwtException("jwt token has expired.");
        }

        var account = _accountRepo.GetById(int.Parse(id));

        var loginSuccessResult = new LsLoginSuccessResult()
        {
            AccountDto = _mapper.Map<AccountDto>(account),
            // LikedTags = account.LikedTags,
            Likes = _mapper.Map<List<LikeDto>>(_likeRepo.GetByLikerId(account.Id))
        };
        return loginSuccessResult;
    }
}