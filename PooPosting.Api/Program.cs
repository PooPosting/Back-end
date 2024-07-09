using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using PooPosting.Application.ActionFilters;
using PooPosting.Application.Authorization;
using PooPosting.Application.Mappers;
using PooPosting.Application.Middleware;
using PooPosting.Application.Models.Configuration;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Dtos.Account.Validators;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Models.Dtos.Picture.Validators;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Models.Queries.Validators;
using PooPosting.Application.Services;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Helpers.Interfaces;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Application.Services.Startup;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Entities;
using PooPosting.Domain.DbContext.Pagination;

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

// Supabase
var supabaseConfig = new SupabaseConfig();
if (builder.Environment.IsDevelopment())
{
    supabaseConfig.Endpoint = builder.Configuration.GetValue<string>("SupabaseConfig:EndpointDev");
    supabaseConfig.Jwt = builder.Configuration.GetValue<string>("SupabaseConfig:JwtDev");
}
if (builder.Environment.IsProduction())
{
    supabaseConfig.Endpoint = builder.Configuration.GetValue<string>("SupabaseConfig:EndpointProd");
    supabaseConfig.Jwt = builder.Configuration.GetValue<string>("SupabaseConfig:JwtProd");
}
builder.Services.AddSingleton(supabaseConfig);
builder.Services.AddHttpClient("SupabaseClient", client =>
{
    client.BaseAddress = new Uri(supabaseConfig.Endpoint + "/storage/v1/s3");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseConfig.Jwt}");
});

// DbContext
builder.Services.AddDbContext<PictureDbContext>(options =>
{
        var connString = builder.Configuration.GetConnectionString(builder.Environment.IsProduction() ? "Prod" : "Dev");
            
        options.UseNpgsql(connString, settings =>
        {
            settings.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            settings.CommandTimeout(30);
        }).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
});

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<PaginationParameters>, QueryValidator>();
builder.Services.AddScoped<IValidator<PictureQueryParams>, SearchQueryValidator>();
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStorageService, StorageService>();

// Helpers
builder.Services.AddScoped<ILikeHelper, LikeHelper>();

// Other stuff
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<PictureSeeder>();
builder.Services.AddScoped<EnvironmentVariableSetter>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

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

// Mapping
AccountMapper.Init(app.Services.GetRequiredService<IHttpContextAccessor>());
PictureMapper.Init(app.Services.GetRequiredService<IHttpContextAccessor>());
CommentMapper.Init(app.Services.GetRequiredService<IHttpContextAccessor>());

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

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "PooPostingAPI");
    c.RoutePrefix = "api/docs";
});

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
        