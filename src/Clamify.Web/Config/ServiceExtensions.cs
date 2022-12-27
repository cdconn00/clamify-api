using System.Reflection;
using System.Text;
using Clamify.Entities;
using Clamify.Entities.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Clamify.Web.Config;

/// <summary>
/// Contains methods for configuring services.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configures identity services.
    /// </summary>
    /// <param name="services">The service object to use for configuration.</param>
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        IdentityBuilder builder = services.AddIdentity<ClamifyUser, IdentityRole<int>>(u => u.User.RequireUniqueEmail = true);
        builder.AddEntityFrameworkStores<ClamifyContext>().AddDefaultTokenProviders();
    }

    /// <summary>
    /// Registers JWT Bearer authentication.
    /// </summary>
    /// <param name="services">The services object to apply to.</param>
    /// <param name="jwtKey">The key to encrypt the JWT with.</param>
    /// <param name="issuer">The issuer of the JWT.</param>
    public static void ConfigureJWT(this IServiceCollection services, string jwtKey, string issuer)
    {
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                };
            });
    }

    /// <summary>
    /// Configures swagger for project.
    /// </summary>
    /// <param name="services">The services object to apply to.</param>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "JWT Authorization header using the Bearer scheme: Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "0auth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Clamify API", Version = "v1" });

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    /// <summary>
    /// Retrieves a secret from the local development or server environment depending on where the application is running.
    /// </summary>
    /// <param name="webApplicationBuilder">The builder used to determine the environment.</param>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The value of the secret requested.</returns>
    public static string GetSecret(WebApplicationBuilder webApplicationBuilder, string secretName)
    {
        if (webApplicationBuilder.Environment.IsDevelopment())
        {
            return webApplicationBuilder.Configuration[secretName] ?? throw new InvalidOperationException($"{secretName} not found.");
        }
        else
        {
            return Environment.GetEnvironmentVariable(secretName) ?? throw new InvalidOperationException($"{secretName} not found.");
        }
    }
}
