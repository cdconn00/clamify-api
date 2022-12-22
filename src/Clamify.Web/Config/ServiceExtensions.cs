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
        IdentityBuilder builder = services.AddIdentityCore<ClamifyUser>(u => u.User.RequireUniqueEmail = true);

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        builder.AddEntityFrameworkStores<ClamifyContext>().AddDefaultTokenProviders();
    }
}
