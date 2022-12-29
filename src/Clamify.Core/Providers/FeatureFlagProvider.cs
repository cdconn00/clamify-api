using Clamify.Core.Providers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Clamify.Core.Providers;

/// <summary>
/// Provides methods to get information about feature flags.
/// </summary>
public class FeatureFlagProvider : IFeatureFlagProvider
{
    private readonly ClamifyContext _context;
    private readonly ILogger<FeatureFlagProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureFlagProvider"/> class.
    /// </summary>
    /// <param name="context">The database context to query.</param>
    /// <param name="logger">Logger object to log exceptions.</param>
    public FeatureFlagProvider(ClamifyContext context, ILogger<FeatureFlagProvider> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<bool> IsFeatureEnabled(string featureName)
    {
        FeatureFlag? foundFeatureFlag = await _context.FeatureFlags.Where(f => f.FeatureName == featureName).FirstOrDefaultAsync();

        if (foundFeatureFlag != null)
        {
            return foundFeatureFlag.IsEnabled;
        }
        else
        {
            _logger.LogError("Tried to retrieve not existent feature flag {FeatureFlag}.", featureName);
            throw new InvalidOperationException($"Feature flag for the feature: {featureName} does not exist.");
        }
    }
}
