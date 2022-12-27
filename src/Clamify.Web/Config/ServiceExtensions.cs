using Clamify.Entities;
using Clamify.Entities.Context;
using Microsoft.AspNetCore.Identity;

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
}
