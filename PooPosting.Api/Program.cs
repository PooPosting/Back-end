using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Dtos.Picture.Validators;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Models.Queries.Validators;
using PooPosting.Application.ActionFilters;
using PooPosting.Application.Authorization;
using PooPosting.Application.Middleware;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Dtos.Account.Validators;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Services;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Helpers.Interfaces;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Application.Services.Startup;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Entities;

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddControllers()
    .AddOData(options => options.Select().Filter().OrderBy());

// Auth
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services
    .AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = "Bearer";
        option.DefaultScheme = "Bearer";
        option.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthorizationHandler, PictureOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AccountOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CommentOperationRequirementHandler>();

// DbContext
builder.Services.AddDbContext<PictureDbContext>(options =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    options
        .UseMySql(connString, ServerVersion.AutoDetect(connString),
            (optionBuilder) =>
            {
                optionBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
        .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
});

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<Query>, QueryValidator>();
builder.Services.AddScoped<IValidator<SearchQuery>, SearchQueryValidator>();
builder.Services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();
// builder.Services.AddScoped<IValidator<ForgetTokensDto>, ForgetTokensDtoValidator>();
// builder.Services.AddScoped<IValidator<LoginWithRefreshTokenDto>, LoginWithRefreshTokenDtoValidator>();

builder.Services.AddScoped<IValidator<UpdateAccountEmailDto>, UpdateAccountEmailDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountPasswordDto>, UpdateAccountPasswordDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountDescriptionDto>, UpdateAccountDescriptionDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountPictureDto>, UpdateAccountPictureDtoValidator>();

builder.Services.AddScoped<IValidator<CreatePictureDto>, CreatePictureDtoValidator>();
builder.Services.AddScoped<IValidator<UpdatePictureNameDto>, UpdatePictureNameDtoValidator>();
builder.Services.AddScoped<IValidator<UpdatePictureDescriptionDto>, UpdatePictureDescriptionDtoValidator>();
builder.Services.AddScoped<IValidator<UpdatePictureTagsDto>, UpdatePictureTagsDtoValidator>();

// Middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<HttpLoggingMiddleware>();

builder.Services.AddScoped<IsUserAdminFilter>();

// Services
builder.Services.AddScoped<IAccountContextService, AccountContextService>();
builder.Services.AddScoped<IAccountPicturesService, AccountPicturesService>();
builder.Services.AddScoped<IPictureLikingService, PictureLikingService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IAuthService, HttpAuthService>();

// Helpers
builder.Services.AddScoped<ILikeHelper, LikeHelper>();

// Other stuff
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<PictureSeeder>();
builder.Services.AddScoped<EnvironmentVariableSetter>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

IdHasher.Configure(builder.Configuration);

// builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddNLog();

var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (envName == "Development")
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "DevCors",
            policy  =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "All",
        policy  =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

var app = builder.Build();

// Configure
DirectoryManager.EnsureAllDirectoriesAreCreated();
app.UseFileServer(
    new FileServerOptions 
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        RequestPath = "/api/wwwroot",
        EnableDefaultFiles = true
    });


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
// app.UseMiddleware<HttpLoggingMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseSwagger();

app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PooPostingAPI"));

var isDev = envName == "Development";

var seeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<PictureSeeder>();
seeder.Seed(isDev);

if (isDev)
{
    app.UseCors("DevCors");
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All,
    RequireHeaderSymmetry = false,
    ForwardLimit = null,
});
app.UseRouting();

app.UseCors("All");

app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
        