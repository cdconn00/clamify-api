using Clamify.Core.Providers;
using Clamify.Core.Providers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;
using Clamify.Tests.Mocks;
using Clamify.Web.Config;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Clamify.Tests.Core.Providers;

/// <summary>
/// Unit test class verifies <see cref="FeatureFlagProvider"/> methods.
/// </summary>
[TestClass]
public class FeatureFlagProviderTests
{
    private ClamifyContext _context;

    private IFeatureFlagProvider GetProvider =>
        new FeatureFlagProvider(_context);

    /// <summary>
    /// Initialize the tests with a mock context.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _context = MockClamifyContextFactory.GenerateMockContext();
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

        GetProvider.IsFeatureEnabled("Test").Should().BeTrue();
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

        GetProvider.IsFeatureEnabled("Test").Should().BeFalse();
    }

    /// <summary>
    /// Unit test verifies case where a non-existent feature throws an exception.
    /// </summary>
    [TestMethod]
    public void IsFeatureEnabled_FeatureDNE_ThrowsException()
    {
        Action act = () =>
        {
            GetProvider.IsFeatureEnabled("Test");
        };

        act.Should().Throw<InvalidOperationException>();
    }
}
