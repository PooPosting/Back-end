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
using PicturesAPI;
using PicturesAPI.ActionFilters;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Middleware;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Validators;
using PicturesAPI.Repos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder();

// NLog: Setup NLog to Dependency injection

    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();

// Configure builder.Services
     
    builder.Services.AddControllers().AddFluentValidation()
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
        var connString = builder.Configuration.GetConnectionString("PictureDbConnection");
        options.UseMySql(connString, ServerVersion.Create(1, 0, 0, ServerType.MariaDb));
    });

    // Validators
    builder.Services.AddScoped<IValidator<AccountQuery>, AccountQueryValidator>();
    builder.Services.AddScoped<IValidator<PictureQuery>, PictureQueryValidator>();
    builder.Services.AddScoped<IValidator<PutAccountDto>, PutAccountDtoValidator>();
    builder.Services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();
    builder.Services.AddScoped<IValidator<LsLoginDto>, LsLoginDtoValidator>();
    builder.Services.AddScoped<IValidator<PatchRestrictedIp>, PatchRestrictedIpValidator>();

    // Middleware
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<UserDataMiddleware>();
    builder.Services.AddScoped<IsIpBannedFilter>();
    builder.Services.AddScoped<IsIpRestrictedFilter>();
    builder.Services.AddScoped<IsUserAdminFilter>();
    
    // builder.Services
    builder.Services.AddScoped<IAccountContextService, AccountContextService>();
    builder.Services.AddScoped<IPictureLikingService, PictureLikingService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IPictureService, PictureService>();
    builder.Services.AddScoped<IPictureCommentService, PictureCommentService>();
    builder.Services.AddScoped<IPopularService, PopularService>();
    builder.Services.AddScoped<IUserAccountService, UserAccountService>();
    builder.Services.AddScoped<ILogsService, LogsService>();
    builder.Services.AddScoped<IRestrictedIpsService, RestrictedIpsService>();

    // Repos
    builder.Services.AddScoped<IAccountRepo, AccountRepo>();
    builder.Services.AddScoped<ILikeRepo, LikeRepo>();
    builder.Services.AddScoped<IPictureRepo, PictureRepo>();
    builder.Services.AddScoped<ICommentRepo, CommentRepo>();
    builder.Services.AddScoped<IRestrictedIpRepo, RestrictedIpRepo>();
    
    // Other stuff
    builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
    builder.Services.AddScoped<PictureSeeder>();
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
DbManagementService.UpdateDb(app);
// Configure

    app.UseCors("FrontEndClient");
    app.UseFileServer(new FileServerOptions  
    {  
        FileProvider = new PhysicalFileProvider(  
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        RequestPath = "/wwwroot",  
        EnableDefaultFiles = true  
    }) ;  

    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<PictureSeeder>();
    seeder.Seed();
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();
    app.UseMiddleware<UserDataMiddleware>();
    // app.UseMiddleware<RestrictedIpsMiddleware>();
    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseSwagger();

    app.UseSwaggerUI(c => 
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicturesAPI v2.0.0"));

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
        