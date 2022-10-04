using Clamify.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Clamify.Tests.Mocks;

/// <summary>
/// Clamify mock context factory to test context changes in unit test project.
/// </summary>
public static class MockClamifyContextFactory
{
    /// <summary>
    /// Generates a mock context for use in unit testing.
    /// </summary>
    /// <returns>A mocked context of the clamify database.</returns>
    public static ClamifyContext GenerateMockContext()
    {
        var options = new DbContextOptionsBuilder<ClamifyContext>()
            .UseInMemoryDatabase(databaseName: "mock_ClamifyContext")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ClamifyContext(options);
        return context;
    }
}
