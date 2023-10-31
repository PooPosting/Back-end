using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PooPosting.Api.Entities;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class HttpAuthService: IAuthService
{
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly PictureDbContext _dbContext;

    public HttpAuthService(
        IPasswordHasher<Account> passwordHasher,
        AuthenticationSettings authenticationSettings,
        PictureDbContext dbContext
    )
    {
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _dbContext = dbContext;
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
        
        var account = await _dbContext.Accounts.AddAsync(newAccount);
        await _dbContext.SaveChangesAsync();
        return IdHasher.EncodeAccountId(account.Entity.Id);
    }

    public async Task<LoginSuccessResult> GenerateJwt(LoginDto dto)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Nickname.ToLower() == dto.Nickname.ToLower());
        if (account is null) throw new UnauthorizedException("Invalid nickname or password");
        
        var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedException("Invalid nickname or password");
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, (account.Id.ToString())),
            new Claim(ClaimTypes.Name, account.Nickname),
            new Claim(ClaimTypes.Role, account.RoleId.ToString()),
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
            Uid = IdHasher.EncodeAccountId(account.Id),
            RoleId = account.RoleId
        };
        
        return loginSuccessResult;
    }

    public async Task<LoginSuccessResult> VerifyJwt(LsLoginDto dto)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadToken(dto.JwtToken) as JwtSecurityToken ?? throw new UnauthorizedException();
            var id = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var account = await _dbContext.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Id == int.Parse(id));
            if (account is null) throw new UnauthorizedException();
            
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, (account.Id.ToString())),
                new(ClaimTypes.Name, account.Nickname),
                new(ClaimTypes.Role, account.Role.Id.ToString()),
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
                AuthToken = tokenHandler.WriteToken(token),
                RoleId = account.RoleId
            };
            return loginSuccessResult;
        }
        catch (Exception)
        {
            throw new UnauthorizedException();
        }

    }
}