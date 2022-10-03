using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Clamify.IntegrationTests.BaseHelpers.StartUp;

/// <summary>
/// Base class to allow for configuration of integration test application.
/// </summary>
public abstract class TestStartupBase
{
    protected readonly IConfiguration Configuration;

    protected TestStartupBase(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Adds neccesary services to the application.
    /// </summary>
    /// <param name="services">The services object to configure.</param>
    public virtual void ConfigureServices(IServiceCollection services)
    {
    }

    /// <summary>
    /// Configures the application to setup routing and use endpoints.
    /// </summary>
    /// <param name="app">The application builder to modify.</param>
    public virtual void Configure(IApplicationBuilder app)
    {
    }
}
