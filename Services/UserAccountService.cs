using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services;

public class UserAccountService : IUserAccountService
{
    private readonly IMapper _mapper;
    private readonly PictureDbContext _dbContext;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly ILogger<UserAccountService> _logger;
    private readonly AuthenticationSettings _authenticationSettings;

    public UserAccountService(
        IMapper mapper,
        PictureDbContext dbContext, 
        IPasswordHasher<Account> passwordHasher, 
        ILogger<UserAccountService> logger, 
        AuthenticationSettings authenticationSettings)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _authenticationSettings = authenticationSettings;
    }
        
    public Guid Create(CreateAccountDto dto)
    {
        var newAccount = new Account()
        {
            Nickname = dto.Nickname,
            Email = dto.Email,
            AccountCreated = DateTime.Now,
            Pictures = new List<Picture>(),
            RoleId = dto.RoleId
        };

        var hashedPassword = _passwordHasher.HashPassword(newAccount, dto.Password);
        newAccount.PasswordHash = hashedPassword;
        _dbContext.Accounts.Add(newAccount);
        _dbContext.SaveChanges();
            
        return newAccount.Id;
    }

    public string GenerateJwt(LoginDto dto)
    {
        var account = _dbContext.Accounts
            .FirstOrDefault(a => a.Nickname == dto.Nickname);
        if (account is null)
            throw new BadRequestException("Invalid nickname or password");

        var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid nickname or password");

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
        return tokenHandler.WriteToken(token);

    }
}