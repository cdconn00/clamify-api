using Clamify.Entities;
using Clamify.IntegrationTests.BaseHelpers.TestBase;
using FluentAssertions;
using Xunit;

namespace Clamify.IntegrationTests.ControllerTests;

/// <summary>
/// Integration test class verifying test cases for the Examples Controller.
/// </summary>
public class ExamplesControllerTests : StandardIntegrationTestBase
{
    /// <summary>
    /// Integration test method verifies GET requests to the Examples endpoint return data as expected.
    /// </summary>
    /// <returns>A task.</returns>
    [Fact]
    public async Task Examples_GivenRequest_ReturnsExamples()
    {
        var context = integrationTestUtilities.GetDbContext();

        var examples = new[]
        {
            new Example(),
            new Example(),
            new Example(),
        };

        context.Examples.AddRange(examples);
        context.SaveChanges();

        var responseObject = await integrationTestUtilities
            .GetResponseFromGetEndpoint<List<Example>>("api/examples/examples", new { });

        responseObject
            .Should()
            .NotBeNull()
            .And
            .HaveCount(3);
    }
}
