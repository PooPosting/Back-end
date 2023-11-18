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
        
        newAccount.ProfilePicUrl =
            Path.Combine("wwwroot", "accounts", "profile_pictures", $"default{new Random().Next(0, 5)}-pfp.webp");
        
        var account = await _dbContext.Accounts.AddAsync(newAccount);
        await _dbContext.SaveChangesAsync();
        return IdHasher.EncodeAccountId(account.Entity.Id);
    }

    public async Task<AuthSuccessResult> GenerateJwt(LoginWithAuthCredsDto dto)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Nickname.ToLower() == dto.Nickname.ToLower());
        if (account is null) throw new UnauthorizedException("Invalid nickname or password");
        
        var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedException("Invalid nickname or password");

        return await GenerateAuthResult(account);
    }
    
    public async Task<AuthSuccessResult> GenerateJwt(LoginWithRefreshTokenDto dto)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == IdHasher.DecodeAccountId(dto.Uid));
        
        if (account is null) throw new NotFoundException();
        if (account.RefreshToken != dto.RefreshToken) throw new UnauthorizedException("Refresh token invalid");
        if (account.RefreshTokenExpires > DateTime.Now) throw new UnauthorizedException("Refresh token invalid");

        return await GenerateAuthResult(account);
    }
    
    public async Task Forget(ForgetTokensDto dto)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == IdHasher.DecodeAccountId(dto.Uid));
        
        if (account is null) throw new NotFoundException();
        if (account.RefreshToken != dto.RefreshToken) throw new UnauthorizedException("Refresh token invalid");
        
        if (account.RefreshTokenExpires < DateTime.Now)
        {
            account.RefreshToken = null;
            account.RefreshTokenExpires = null;
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task<AuthSuccessResult> GenerateAuthResult(Account account)
    {
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
        

        var refreshToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

        account.RefreshToken = refreshToken;
        account.RefreshTokenExpires = DateTime.Now.AddDays(_authenticationSettings.RefreshTokenExpireDays);

        await _dbContext.SaveChangesAsync();
            
        var tokenHandler = new JwtSecurityTokenHandler();
        var authSuccessResult = new AuthSuccessResult()
        {
            AuthToken = tokenHandler.WriteToken(token),
            Uid = IdHasher.EncodeAccountId(account.Id),
            RoleId = account.RoleId,
            RefreshToken = refreshToken
        };
        
        return authSuccessResult;
    }
}