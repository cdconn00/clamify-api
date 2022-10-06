using Clamify.Core.Providers;
using Clamify.Core.Providers.Interfaces;
using Clamify.Core.Writers;
using Clamify.Core.Writers.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clamify.Core.Configuration;

/// <summary>
/// Provides methods to register Core project dependencies.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Parent method to register methods for different service groups.
    /// </summary>
    /// <param name="services">Services object where dependencies are added.</param>
    /// <param name="configuration">Contains application configuration properties.</param>
#pragma warning disable IDE0060 // Remove unused parameter, will be implemented in future
    public static void RegisterCoreDependencies(this IServiceCollection services, IConfiguration configuration)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        services.RegisterCaching();
        services.RegisterProviders();
        services.RegisterWriters();
    }

    private static void RegisterCaching(this IServiceCollection services)
    {
        services.AddSingleton<IMemoryCache, MemoryCache>(_ => new MemoryCache(new MemoryCacheOptions()));
        services.AddSingleton(_ => new MemoryCacheEntryOptions
        {
            // Default cache expiration time of 12 hours, since user name and part number data is unlikely to change very often
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(720),
        });
    }

    private static void RegisterProviders(this IServiceCollection services)
    {
        services.AddTransient<IExampleProvider, ExampleProvider>();
    }

    private static void RegisterWriters(this IServiceCollection services)
    {
        services.AddTransient<IExampleWriter, ExampleWriter>();
    }
}
