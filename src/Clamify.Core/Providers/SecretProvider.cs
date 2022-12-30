using Clamify.Core.Providers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Clamify.Core.Providers;

/// <summary>
/// Provides methods to retrieve secrets.
/// </summary>
public class SecretProvider : ISecretProvider
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecretProvider"/> class.
    /// </summary>
    /// <param name="configuration">The configuration to utilize in checking/retrieving secrets.</param>
    public SecretProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Retrieves a secret from the local development or server environment depending on where the application is running.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The value of the secret requested.</returns>
    public string GetSecret(string secretName)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            return _configuration[secretName] ?? throw new InvalidOperationException($"{secretName} not found.");
        }
        else
        {
            return Environment.GetEnvironmentVariable(secretName) ?? throw new InvalidOperationException($"{secretName} not found.");
        }
    }
}
