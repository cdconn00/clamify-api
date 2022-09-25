using Clamify.Entities;

namespace Clamify.Core.Providers.Interfaces;

/// <summary>
/// Contract defining methods to retrieve examples.
/// </summary>
public interface IExampleProvider
{
    /// <summary>
    /// Gets a list of examples from the database context.
    /// </summary>
    /// <returns>A list of example from the DB (if any).</returns>
    public IEnumerable<Example> Get();
}
