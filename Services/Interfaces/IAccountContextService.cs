﻿#nullable enable
using System.Security.Claims;
using PicturesAPI.Entities;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountContextService
{
    string GetEncodedAccountId();
    ClaimsPrincipal User { get; }
    int? TryGetAccountId();
    int GetAccountId();
    Task<Account> GetAccountAsync();
    int GetAccountRole();
}