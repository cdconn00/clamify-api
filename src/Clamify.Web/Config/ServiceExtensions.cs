using System.Text;
using Clamify.Entities;
using Clamify.Entities.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
