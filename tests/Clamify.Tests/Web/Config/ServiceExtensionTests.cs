using Clamify.Web.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Clamify.Tests.Web.Config;

/// <summary>
/// Unit test class verifying <see cref="ServiceExtensions"/> methods.
/// </summary>
[TestClass]
public class ServiceExtensionTests
{
    private WebApplicationBuilder _builder;

    /// <summary>
    /// Unit test verifies a development value is used in development.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsDevelopment_ReturnsDevelopmentValue()
    {
        string expectedValue = "development";

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        _builder = WebApplication.CreateBuilder();
        AddSecretValues();

        ServiceExtensions.GetSecret(_builder, "secret").Should().BeEquivalentTo(expectedValue);
    }

    /// <summary>
    /// Unit test verifies a production value is used in production.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsProduction_ReturnsProductionValue()
    {
        string expectedValue = "production";

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
        _builder = WebApplication.CreateBuilder();
        AddSecretValues();

        ServiceExtensions.GetSecret(_builder, "secret").Should().BeEquivalentTo(expectedValue);
    }

    /// <summary>
    /// Unit test verifies a non-existent development value throws an exception.
    /// </summary>
    [TestMethod]
    public void GetSecret_IsDevelopmentAndNoValue_ThrowsException()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        _builder = WebApplication.CreateBuilder();

        _builder.Configuration["secret"] = null;

        Action act = () =>
        {
            ServiceExtensions.GetSecret(_builder, "secret");
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
        _builder = WebApplication.CreateBuilder();

        // Make sure the environment variable is null
        Environment.SetEnvironmentVariable("secret", null);

        Action act = () =>
        {
            ServiceExtensions.GetSecret(_builder, "secret");
        };

        act.Should().Throw<InvalidOperationException>();
    }

    private void AddSecretValues()
    {
        _builder.Configuration["secret"] = "development";
        Environment.SetEnvironmentVariable("secret", "production");
    }
}
