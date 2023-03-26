using System.Reflection;
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
using NLog.Web;
using PooPosting.Api;
using PooPosting.Api.ActionFilters;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Factories;
using PooPosting.Api.Factories.Interfaces;
using PooPosting.Api.Middleware;
using PooPosting.Api.Models.Configuration;
using PooPosting.Api.Models.Dtos;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos.Account.Validators;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Dtos.Picture.Validators;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Models.Queries.Validators;
using PooPosting.Api.Models.Validators;
using PooPosting.Api.Repos;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Helpers.Interfaces;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Services.Startup;

var builder = WebApplication.CreateBuilder();

// NLog: Setup NLog to Dependency injection

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Configure builder.Services

builder.Services.AddControllers().AddFluentValidation()
    .AddOData(options => options.Select().Filter().OrderBy());

var sitemapSettings = new SitemapSettings();
builder.Configuration.GetSection("SitemapSettings").Bind(sitemapSettings);
builder.Services.AddSingleton(sitemapSettings);

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
    var connString = builder.Configuration.GetConnectionString("PictureDbConnection");
    options
        .UseMySql(connString, ServerVersion.AutoDetect(connString),
            (optionBuilder) =>
            {
                optionBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
        .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
});

// Validators
builder.Services.AddScoped<IValidator<CustomQuery>, CustomQueryValidator>();
builder.Services.AddScoped<IValidator<Query>, QueryValidator>();
builder.Services.AddScoped<IValidator<PersonalizedQuery>, PersonalizedQueryValidator>();
builder.Services.AddScoped<IValidator<LsLoginDto>, LsLoginDtoValidator>();
builder.Services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();
builder.Services.AddScoped<IValidator<PatchRestrictedIp>, PatchRestrictedIpValidator>();

builder.Services.AddScoped<IValidator<UpdateAccountEmailDto>, UpdateAccountEmailDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountPasswordDto>, UpdateAccountPasswordDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountDescriptionDto>, UpdateAccountDescriptionDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountPictureDto>, UpdateAccountPictureDtoValidator>();

builder.Services.AddScoped<IValidator<UpdatePictureNameDto>, UpdatePictureNameDtoValidator>();
builder.Services.AddScoped<IValidator<UpdatePictureDescriptionDto>, UpdatePictureDescriptionDtoValidator>();
builder.Services.AddScoped<IValidator<UpdatePictureTagsDto>, UpdatePictureTagsDtoValidator>();

// Middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<UserDataMiddleware>();
builder.Services.AddScoped<IsUserAdminFilter>();

// builder.Services
builder.Services.AddScoped<IAccountContextService, AccountContextService>();
builder.Services.AddScoped<IPictureLikingService, PictureLikingService>();
builder.Services.AddScoped<IAccountService, HttpAccountService>();
builder.Services.AddScoped<IPictureService, HttpPictureService>();
builder.Services.AddScoped<IPopularService, HttpPopularService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IAuthService, HttpAuthService>();
builder.Services.AddScoped<ILogsService, LogsService>();
builder.Services.AddScoped<IRestrictedIpsService, RestrictedIpsService>();

// Helpers
builder.Services.AddScoped<ITagHelper, TagHelper>();
builder.Services.AddScoped<ILikeHelper, LikeHelper>();
builder.Services.AddScoped<IPictureHelper, PictureHelper>();

// Repos
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<ILikeRepo, LikeRepo>();
builder.Services.AddScoped<IPictureRepo, PictureRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddScoped<IRestrictedIpRepo, RestrictedIpRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<IPopularRepo, PopularRepo>();

// Factories
builder.Services.AddScoped<ISitemapFactory, SitemapFactory>();

// Other stuff
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<PictureSeeder>();
builder.Services.AddScoped<EnvironmentVariableSetter>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
        {
            var originList = new List<string>();
            foreach (var origin in builder.Configuration["AllowedOrigins"].Split(','))
            {
                originList.Add(origin);
                Console.WriteLine($"Built with CORS origin: {origin}");
            }
            policyBuilder
                .WithOrigins(originList.ToArray())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

var app = builder.Build();
// Configure
app.UseCors("FrontEndClient");
DirectoryManager.EnsureAllDirectoriesAreCreated();
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/api/wwwroot",
    EnableDefaultFiles = true
});

using (var scope = app.Services.CreateScope()) {
    if (app.Environment.IsDevelopment())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<PictureSeeder>();
        seeder.Seed();

        app.UseDeveloperExceptionPage();
    }
    var envSetter = scope.ServiceProvider.GetRequiredService<EnvironmentVariableSetter>();
    envSetter.Set();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseMiddleware<UserDataMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseSwagger();

app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicturesAPI v3.2.0"));

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All,
    RequireHeaderSymmetry = false,
    ForwardLimit = null,
});
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
        