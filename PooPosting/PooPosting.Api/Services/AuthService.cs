using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PooPosting.Api.Entities;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services;

public class HttpAuthService: IAuthService
{
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IAccountRepo _accountRepo;

    public HttpAuthService(
        IPasswordHasher<Account> passwordHasher,
        AuthenticationSettings authenticationSettings,
        IAccountRepo accountRepo
        )
    {
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _accountRepo = accountRepo;
    }

    public async Task<string> RegisterAccount(CreateAccountDto dto)
    {
        var newAccount = new Account()
        {
            Nickname = dto.Nickname,
            Email = dto.Email
        };

        var hashedPassword = _passwordHasher.HashPassword(newAccount, dto.Password);
        newAccount.PasswordHash = hashedPassword;

        newAccount.BackgroundPicUrl =
            Path.Combine("wwwroot", "accounts", "background_pictures", $"default{new Random().Next(0, 5)}-bgp.webp");

        newAccount.ProfilePicUrl =
            Path.Combine("wwwroot", "accounts", "profile_pictures", $"default{new Random().Next(0, 5)}-pfp.webp");

        var account = await _accountRepo.InsertAsync(newAccount);
        return IdHasher.EncodeAccountId(account.Id);
    }

    public async Task<LoginSuccessResult> GenerateJwt(LoginDto dto)
    {
        var account = await _accountRepo.GetByNickAsync(dto.Nickname, a => a.Role);

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
            AuthToken = tokenHandler.WriteToken(token),
            Uid = IdHasher.EncodeAccountId(account.Id)
        };

        return loginSuccessResult;

    }

    public async Task<LoginSuccessResult> VerifyJwt(LsLoginDto dto)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadToken(dto.JwtToken) as JwtSecurityToken ?? throw new InvalidAuthTokenException();
            var id = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var account = await _accountRepo.GetByIdAsync(int.Parse(id)) ?? throw new InvalidAuthTokenException();
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
                Uid = IdHasher.EncodeAccountId(int.Parse(id)),
                AuthToken = tokenHandler.WriteToken(token)
            };
            return loginSuccessResult;
        }
        catch (Exception)
        {
            throw new InvalidAuthTokenException();
        }




    }
}