using System.Security.Claims;
using System.Text;
using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Services;
using Services.Services.EmailServices;
using Services.Settings;
using Services.Utils;

namespace BePrácticasLaborales;

public static class ServiceCollectionExtention
{
    public static IServiceCollection SetAuthentication(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var a  = configuration.GetSection("Authentication").GetSection("Password").GetValue<int>("RequiredLength");
    services.AddIdentity<User, IdentityRole<int>>(options =>
    { 
        
        options.Password.RequiredLength =
            configuration.GetSection("Authentication").GetSection("Password").GetValue<int>("RequiredLength");
        options.Password.RequireNonAlphanumeric = configuration.GetSection("Authentication").GetSection("Password")
            .GetValue<bool>("RequireNonAlphanumeric");
        options.Password.RequireDigit =
            configuration.GetSection("Authentication").GetSection("Password").GetValue<bool>("RequireDigit");
        options.Password.RequireLowercase =
            configuration.GetSection("Authentication").GetSection("Password").GetValue<bool>("RequireLowercase");
        options.Password.RequireUppercase =
            configuration.GetSection("Authentication").GetSection("Password").GetValue<bool>("RequireUppercase");
            
        options.ClaimsIdentity = new ClaimsIdentityOptions
        {
            EmailClaimType = ClaimTypes.Email,
            RoleClaimType = ClaimTypes.Role,
            UserIdClaimType = ClaimTypes.NameIdentifier,
            UserNameClaimType = ClaimTypes.Name 
        };
        //change this emailconfirm
        options.SignIn.RequireConfirmedEmail = false;
    })
        .AddEntityFrameworkStores<EntityDbContext>()
        .AddDefaultTokenProviders()
        .AddApiEndpoints();

    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>  {
                    return expires > DateTime.UtcNow;
                },
                ValidAudience = configuration["JwtSettings:Audience"],
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                
            };
        });
    
    /*
     services.AddAuthorization(options =>
    {
        options.AddPolicy("IsAdmin", policy => policy.RequireClaim(ClaimTypes.Role, 
            RoleNames.Admin.ToUpper()));
    });
    */
    
    
    return services;
}

    public static IServiceCollection SetServices(
        this IServiceCollection services,
        IConfiguration configuration)   
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<TokenUtil>();
        services.AddScoped<EmailService>();
        services.AddScoped<ImportDbServices>();
        services.AddScoped<OrganizationServices>();
        services.AddScoped<SurveyServices>();
        services.AddScoped<UserServicers>();
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        return services;
    }
    
    public static IServiceCollection SetCors(
        this IServiceCollection services,
        IConfiguration configuration,
        string policyName)
    {
        var origins = configuration
            .GetSection("CorsAllowedOrigins")
            .Get<string[]>();

        if (origins is null)
            throw new NullReferenceException(nameof(origins));

        services.AddCors(options =>
        {
            options.AddPolicy(name: policyName, builder =>
            {
                builder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}