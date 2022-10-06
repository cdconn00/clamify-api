using Clamify.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clamify.RequestHandling.Configuration;

/// <summary>
/// Provides methods to register Clamify.RequestHandling project dependencies.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Parent method to register different service groups.
    /// </summary>
    /// <param name="services">Services object where dependencies are added.</param>
    /// <param name="configuration">Contains application configuration properties.</param>
    public static void RegisterRequestHandlingDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterCoreDependencies(configuration);
        services.RegisterProviders();
    }

#pragma warning disable IDE0060 // Remove unused parameter, will be implemented later.
    private static void RegisterProviders(this IServiceCollection services)
#pragma warning restore IDE0060 // Remove unused parameter
    {
    }
}
