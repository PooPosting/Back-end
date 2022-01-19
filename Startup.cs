using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Interfaces;
using PicturesAPI.Middleware;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Validators;
using PicturesAPI.Services;

namespace PicturesAPI;

public class Startup
{
    
    // public Startup(IHostEnvironment env)
    // {
    //     var builder = new ConfigurationBuilder()
    //         .SetBasePath(env.ContentRootPath)
    //         .AddJsonFile("appsettings.json")
    //         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    //         .AddEnvironmentVariables();
    //     
    //     Console.WriteLine(env.EnvironmentName);
    //
    //     Configuration = builder.Build();
    // }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddControllers().AddFluentValidation()
            .AddOData(options => options.Select().Filter().OrderBy());

        // Auth
        var authenticationSettings = new AuthenticationSettings();
        Configuration.GetSection("Authentication").Bind(authenticationSettings);
        services.AddSingleton(authenticationSettings);
        services
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
        services.AddAuthorization();
        services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();


        // DbContext
        services.AddDbContext<PictureDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("PictureDbConnection"));
        });

        // Validators
        services.AddScoped<IValidator<AccountQuery>, AccountQueryValidator>();
        services.AddScoped<IValidator<PictureQuery>, PictureQueryValidator>();
        services.AddScoped<IValidator<PutAccountDto>, PutAccountDtoValidator>();
        services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();

        // Middleware
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<RequestTimeMiddleware>();
        
        // Services
        services.AddScoped<IAccountContextService, AccountContextService>();
        services.AddScoped<IPictureLikingService, PictureLikingService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        
        // Other stuff
        services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
        services.AddScoped<PictureSeeder>();
        services.AddAutoMapper(this.GetType().Assembly);
        services.AddHttpContextAccessor();
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PictureSeeder seeder)
    {
        seeder.Seed();
        if (env.IsDevelopment())
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
    }
}