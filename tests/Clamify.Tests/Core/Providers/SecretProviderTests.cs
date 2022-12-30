using Clamify.Core.Providers;
using Clamify.Core.Providers.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Clamify.Tests.Core.Providers;

/// <summary>
/// Unit test class verifying <see cref="SecretProvider"/> methods.
/// </summary>
[TestClass]
public class SecretProviderTests
{
    private IConfiguration _configuration;

    private ISecretProvider GetProvider =>
        new SecretProvider(_configuration);

    /// <summary>
    /// Initialize the tests with a mock objects.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _configuration = Mock.Of<IConfiguration>();
    }

    /// <summary>
    /// Unit test verifies a development value is used in development.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsDevelopment_ReturnsDevelopmentValue()
    {
        string expectedValue = "development";

        var inMemorySettings = new Dictionary<string, string> { { "secret", expectedValue }, };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        GetProvider.GetSecret("secret").Should().BeEquivalentTo(expectedValue);
    }

    /// <summary>
    /// Unit test verifies a production value is used in production.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsProduction_ReturnsProductionValue()
    {
        string expectedValue = "production";

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
        Environment.SetEnvironmentVariable("secret", "production");

        GetProvider.GetSecret("secret").Should().BeEquivalentTo(expectedValue);
    }

    /// <summary>
    /// Unit test verifies a non-existent development value throws an exception.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsDevelopmentAndNoValue_ThrowsException()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        Action act = () =>
        {
            GetProvider.GetSecret("secret");
        };

        act.Should().Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Unit test verifies a non-existent production value throws an exception.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsProductionAndNoValue_ThrowsException()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");

        // Make sure the environment variable is null
        Environment.SetEnvironmentVariable("secret", null);

        Action act = () =>
        {
            GetProvider.GetSecret("secret");
        };

        act.Should().Throw<InvalidOperationException>();
    }
}
