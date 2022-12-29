namespace Clamify.Core.Providers.Interfaces;

/// <summary>
/// Defines methods for interacting with feature flag information.
/// </summary>
public interface IFeatureFlagProvider
{
    /// <summary>
    /// Queries if the feature is enabled or not.
    /// </summary>
    /// <param name="featureName">The feature to check.</param>
    /// <returns>If the feature is enabled or not.</returns>
    public bool IsFeatureEnabled(string featureName);
}
