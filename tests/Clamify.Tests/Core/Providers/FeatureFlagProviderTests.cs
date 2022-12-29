using Clamify.Core.Providers;
using Clamify.Core.Providers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;
using Clamify.Tests.Mocks;
using Clamify.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Clamify.Tests.Core.Providers;

/// <summary>
/// Unit test class verifies <see cref="FeatureFlagProvider"/> methods.
/// </summary>
[TestClass]
public class FeatureFlagProviderTests
{
    private ClamifyContext _context;
    private ILogger<FeatureFlagProvider> _logger;

    private IFeatureFlagProvider GetProvider =>
        new FeatureFlagProvider(_context, _logger);

    /// <summary>
    /// Initialize the tests with a mock context.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _context = MockClamifyContextFactory.GenerateMockContext();
        _logger = Mock.Of<ILogger<FeatureFlagProvider>>();
    }

    /// <summary>
    /// Verifies the database in the context is deleted.
    /// </summary>
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    /// <summary>
    /// Unit test verifies case where an enabled feature returns true.
    /// </summary>
    [TestMethod]
    public void IsFeatureEnabled_FeatureEnabled_ReturnsTrue()
    {
        _context.FeatureFlags.Add(new FeatureFlag
        {
            FeatureName = "Test",
            IsEnabled = true,
        });

        _context.SaveChanges();

        GetProvider.IsFeatureEnabled("Test").Result.Should().BeTrue();
    }

    /// <summary>
    /// Unit test verifies case where an disabled feature returns false.
    /// </summary>
    [TestMethod]
    public void IsFeatureEnabled_FeatureDisabled_ReturnsFalse()
    {
        _context.FeatureFlags.Add(new FeatureFlag
        {
            FeatureName = "Test",
            IsEnabled = false,
        });

        _context.SaveChanges();

        GetProvider.IsFeatureEnabled("Test").Result.Should().BeFalse();
    }

    /// <summary>
    /// Unit test verifies case where a non-existent feature throws an exception.
    /// </summary>
    [TestMethod]
    public void IsFeatureEnabled_FeatureDNE_ThrowsException()
    {
        Action act = () =>
        {
            var res = GetProvider.IsFeatureEnabled("Test").Result;
        };

        act.Should().Throw<InvalidOperationException>();
        LoggerCallHelpers.VerifyErrorCall(_logger);
    }
}
