using System.Reflection;
using Clamify.Entities.Context;
using Clamify.RequestHandling.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Clamify.IntegrationTests.BaseHelpers.StartUp;

/// <summary>
/// Startup class configuring services application for integration testing.
/// </summary>
public class IntegrationTestStartup : TestStartupBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTestStartup"/> class.
    /// </summary>
    /// <param name="configuration">Configuration object to utilize.</param>
    public IntegrationTestStartup(IConfiguration configuration)
    : base(configuration)
    {
    }

    /// <inheritdoc/>
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(options => options.AddDebug());

        services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Clamify.Web")));

        services.AddDbContext<ClamifyContext>(options =>
            options.UseNpgsql(
                Configuration.GetConnectionString("Clamify"),
                x => x.MigrationsAssembly("Clamify.IntegrationTests")));

        services.AddHttpContextAccessor();
        services.AddTransient(x => x.GetService<IHttpContextAccessor>().HttpContext.User);

        services.RegisterRequestHandlingDependencies(Configuration);
    }

    /// <inheritdoc/>
    public override void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
        });
    }
}
