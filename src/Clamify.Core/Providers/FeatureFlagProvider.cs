using Clamify.Core.Providers.Interfaces;
using Clamify.Entities.Context;

namespace Clamify.Core.Providers;

/// <summary>
/// Provides methods to get information about feature flags.
/// </summary>
public class FeatureFlagProvider : IFeatureFlagProvider
{
    private readonly ClamifyContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureFlagProvider"/> class.
    /// </summary>
    /// <param name="context">The database context to query.</param>
    public FeatureFlagProvider(ClamifyContext context)
    {
        _context = context;
    }

    public bool IsFeatureEnabled(string featureName) => throw new NotImplementedException();
}
