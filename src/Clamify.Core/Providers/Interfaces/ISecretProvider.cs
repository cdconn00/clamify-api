namespace Clamify.Core.Providers.Interfaces;

/// <summary>
/// Contract defining methods to retrieve secrets from the envrionment/config.
/// </summary>
public interface ISecretProvider
{
    /// <summary>
    /// Retrieves a secret from the local development or server environment depending on where the application is running.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The value of the secret requested.</returns>
    public string GetSecret(string secretName);
}
