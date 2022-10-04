using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clamify.IntegrationTests.BaseHelpers.StartUp;

/// <summary>
/// Base class to allow for configuration of integration test application.
/// </summary>
public abstract class TestStartupBase
{
    /// <summary>
    /// Configuration object for the <see cref="TestStartupBase"/>.
    /// </summary>
#pragma warning disable SA1401 // Fields should be private
    protected readonly IConfiguration Configuration;
#pragma warning restore SA1401 // Fields should be private

    /// <summary>
    /// Initializes a new instance of the <see cref="TestStartupBase"/> class.
    /// </summary>
    /// <param name="configuration">The configuration object to utilize.</param>
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
