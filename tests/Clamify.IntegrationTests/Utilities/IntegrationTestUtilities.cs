using System.Text;
using Clamify.Entities.Context;
using Clamify.IntegrationTests.BaseHelpers.StartUp;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clamify.IntegrationTests.Utilities;

/// <summary>
/// Utility functions class for integrationt test setup and execution.
/// </summary>
/// <typeparam name="TStartup">Startup class containing configuration.</typeparam>
public class IntegrationTestUtilities<TStartup>
    where TStartup : TestStartupBase
{
    private readonly PostgreSqlTestcontainer _dbContainer =
        new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "mydb",
                Username = "test",
                Password = "test",
            }).Build();

    private TestServer _testServer;

    /// <summary>
    /// Sets up the test server.
    /// </summary>
    /// <returns>A task.</returns>
    public async Task InitializeTestServer()
    {
        await _dbContainer.StartAsync();

        var webHostBuilder = new WebHostBuilder()
        .UseStartup<TStartup>()
        .ConfigureAppConfiguration(config =>
            config
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:Clamify", _dbContainer.ConnectionString),
                }))
        .ConfigureLogging((_, loggingBuilder) =>
        {
            loggingBuilder.AddDebug();
        });

        var testServer = new TestServer(webHostBuilder);
        await MigrateDatabase(testServer);
        testServer.BaseAddress = new Uri("http://0.0.0.0:5000/");
        _testServer = testServer;
    }

    /// <summary>
    /// Gets the Clamify Database Context.
    /// </summary>
    /// <returns>A <see cref="ClamifyContext"/>.</returns>
    public ClamifyContext GetDbContext() =>
        GetScope().ServiceProvider.GetRequiredService<ClamifyContext>();

    /// <summary>
    /// Gets an HttpClient to use in making requests.
    /// </summary>
    /// <returns>An <see cref="HttpClient"/>.</returns>
    public HttpClient GetClient()
    {
        var client = _testServer.CreateClient();
        client.Timeout = TimeSpan.FromMinutes(1);

        return client;
    }

    /// <summary>
    /// Deletes the database.
    /// </summary>
    /// <returns>A task.</returns>
    public async Task DeleteDatabaseContext()
    {
        using var scope = GetScope();
        var serviceProvider = scope.ServiceProvider;
        using var context = serviceProvider.GetRequiredService<ClamifyContext>();

        await context.Database.EnsureDeletedAsync();
    }

    /// <summary>
    /// Preforms a GET request and returns a deserialized response.
    /// </summary>
    /// <typeparam name="TResponse">The type that will be returned by the endpoint.</typeparam>
    /// <param name="uri">The location of the endpoint.</param>
    /// <param name="request">The payload of the GET request.</param>
    /// <returns>The deserialized response from the endpoint.</returns>
    public async Task<TResponse> GetResponseFromGetEndpoint<TResponse>(string uri, object? request)
    {
        uri += "?";

        foreach (var property in request.GetType().GetProperties())
        {
            uri += $"{property.Name}={property.GetValue(request)}&";
        }

        // Make the URI a little prettier
        uri = uri.TrimEnd('?').TrimEnd('&');

        var response = await GetClient().GetAsync(new Uri(uri, UriKind.Relative));
        var responseContext = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseContext);
    }

    /// <summary>
    /// Preforms a POST request and returns a deserialized response.
    /// </summary>
    /// <typeparam name="TResponse">The type that will be returned by the endpoint.</typeparam>
    /// <param name="uri">The location of the endpoint.</param>
    /// <param name="request">The payload of the POST request.</param>
    /// <returns>The deserialized response from the endpoint.</returns>
    public async Task<TResponse> GetResponseObjectFromPost<TResponse>(string uri, object request)
    {
        var response = await GetClient().PostAsync(
            new Uri(uri, UriKind.Relative),
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        var responseContext = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseContext);
    }

    /// <summary>
    /// Performs a migration on the database to create schema.
    /// </summary>
    /// <param name="testServer">The test server to get the context from.</param>
    /// <returns>A task.</returns>
    private static async Task MigrateDatabase(TestServer testServer)
    {
        var context = testServer.Host.Services.GetRequiredService<ClamifyContext>();
        await context.Database.MigrateAsync();
    }

    private IServiceScope GetScope()
    {
        return _testServer.Host.Services.CreateScope();
    }
}
