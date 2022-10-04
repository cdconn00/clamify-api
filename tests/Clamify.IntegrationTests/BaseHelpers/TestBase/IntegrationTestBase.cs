using Clamify.IntegrationTests.BaseHelpers.StartUp;
using Clamify.IntegrationTests.Utilities;
using Xunit;

namespace Clamify.IntegrationTests.BaseHelpers.TestBase;

/// <summary>
/// Base class handling lifetime functionality.
/// </summary>
/// <typeparam name="TApplicationStartup">Application startup object to utilize in starting the test server.</typeparam>
public abstract class IntegrationTestBase<TApplicationStartup> : IAsyncLifetime where TApplicationStartup : TestStartupBase
{
    protected IntegrationTestUtilities<TApplicationStartup> IntegrationTestUtilities;

    /// <summary>
    /// Starts up the test server and waits for it to be initialized..
    /// </summary>
    /// <returns>A task.</returns>
    public virtual async Task InitializeAsync()
    {
        IntegrationTestUtilities = new IntegrationTestUtilities<TApplicationStartup>();
        await IntegrationTestUtilities.InitializeTestServer();
    }

    /// <summary>
    /// Disposes of the test server and its resources.
    /// </summary>
    /// <returns>A task.</returns>
    public virtual async Task DisposeAsync()
    {
        await IntegrationTestUtilities.DeleteDatabaseContext();
    }
}
