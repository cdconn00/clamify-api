using Clamify.Core.Providers;
using Clamify.Core.Providers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;
using Clamify.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Clamify.Tests.Core.Providers;

/// <summary>
/// Unit test class verifies example provider functions.
/// </summary>
[TestClass]
public class ExampleProviderTests
{
    private ClamifyContext _context;

    private IExampleProvider GetProvider =>
        new ExampleProvider(_context);

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
    /// Unit test verifies case where a valid request returns the expected number of examples.
    /// </summary>
    [TestMethod]
    public void Get_GivenRequest_ReturnsExamples()
    {
        var expectedResult = new[]
        {
            new Example
            {
                ExampleID = 1,
            },
        };

        _context.Examples.AddRange(expectedResult);
        _context.SaveChanges();

        GetProvider
            .Get()
            .Should()
            .BeEquivalentTo(expectedResult);
    }
}
