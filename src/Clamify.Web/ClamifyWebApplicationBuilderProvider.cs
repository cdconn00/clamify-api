using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Clamify.Entities.Context;
using Clamify.RequestHandling.Configuration;
using Serilog.Templates;
using Serilog.Templates.Themes;
using System.Reflection;

namespace Clamify.Web;

/// <summary>
/// Static provider class handling builder configuration for the Web project.
/// </summary>
public static class ClamifyWebApplicationBuilderProvider
{
    /// <summary>
    /// Provider function to return a web application builder for the web application to be built with.
    /// </summary>
    /// <param name="strings">Argument to create a web application builder.</param>
    /// <returns>WebApplicationBuilder configured for the application.</returns>
    public static WebApplicationBuilder Get(string[] strings)
    {
        var webApplicationBuilder = WebApplication.CreateBuilder(strings);

        webApplicationBuilder.Services.RegisterRequestHandlingDependencies(webApplicationBuilder.Configuration);

        webApplicationBuilder.Services.AddDbContext<ClamifyContext>(o =>
        {
            o.UseNpgsql(webApplicationBuilder.Configuration.GetConnectionString(webApplicationBuilder.Configuration.GetValue<string>("Database:ConnectionString")),
                options => { options.EnableRetryOnFailure(); });

            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name &&
                            level == LogLevel.Information
                        ).AddConsole();
                });

                o.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
            }
        });

        webApplicationBuilder.Services.AddCors(options =>
            options.AddPolicy("AllowAllPolicy",
                b => b.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            )
        );
        webApplicationBuilder.Services.AddControllers();
        webApplicationBuilder.Services.AddHttpContextAccessor();

        webApplicationBuilder.Services.AddHealthChecks();

        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        webApplicationBuilder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Clamify API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "JWT Authorization header using the Bearer scheme: Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        webApplicationBuilder.Host.UseSerilog();
        webApplicationBuilder.Host.ConfigureLogging((hostContext, logBuilder) =>
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion(true)
                .MinimumLevel.Information()
                .MinimumLevel.Override("System", new Serilog.Core.LoggingLevelSwitch(LogEventLevel.Warning))
                .MinimumLevel.Override("Microsoft", new Serilog.Core.LoggingLevelSwitch(LogEventLevel.Warning))
                .MinimumLevel
                .Override("Microsoft.EntityFrameworkCore.Database.Command",
                    LogEventLevel.Warning) // Disable EFCore from logging everything.
                .Destructure.ToMaximumDepth(6)

                .ReadFrom.Configuration(hostContext.Configuration)

                .WriteTo.Conditional(
                    _ => hostContext.HostingEnvironment.EnvironmentName == "Development",
                    wt => wt.Console(formatter: new ExpressionTemplate(
                        template:
                        "[{@t:HH:mm:ss} ({Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)})] {@l:u3} - {@m}\n{@x}",
                        formatProvider: null,
                        nameResolver: null, theme: TemplateTheme.Literate,
                        applyThemeWhenOutputIsRedirected: false)))
                .CreateLogger();

            logBuilder.AddSerilog(logger: Log.Logger, dispose: true);
        });

        return webApplicationBuilder;
    }
}
