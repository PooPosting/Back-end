using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using PicturesAPI;
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
    builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, AccountOperationRequirementHandler>();


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

    // Middleware
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    
    // builder.Services
    builder.Services.AddScoped<IAccountContextService, AccountContextService>();
    builder.Services.AddScoped<IPictureLikingService, PictureLikingService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IPictureService, PictureService>();
    builder.Services.AddScoped<IUserAccountService, UserAccountService>();
    
    // Repos
    builder.Services.AddScoped<IAccountRepo, AccountRepo>();
    builder.Services.AddScoped<ILikeRepo, LikeRepo>();
    builder.Services.AddScoped<IPictureRepo, PictureRepo>();
    
    // Other stuff
    builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IClassifyNsfw, ClassifyNsfw>();
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
// DbManagementService.MigrationInit(app);
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
    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseSwagger();

    app.UseSwaggerUI(c => 
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicturesAPI v1"));

    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
        