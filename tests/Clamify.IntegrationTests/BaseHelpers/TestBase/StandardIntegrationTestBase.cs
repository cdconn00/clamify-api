using Clamify.IntegrationTests.BaseHelpers.StartUp;

namespace Clamify.IntegrationTests.BaseHelpers.TestBase;

/// <summary>
/// Base class to apply to test classes to facilitate access to integration test resources.
/// </summary>
public abstract class StandardIntegrationTestBase : IntegrationTestBase<IntegrationTestStartup>
{
}
