using Clamify.Core.Providers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;

namespace Clamify.Core.Providers;

/// <summary>
/// Implementation of provider to get examples from the DB.
/// </summary>
public class ExampleProvider : IExampleProvider
{
    private readonly ClamifyContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleProvider"/> class.
    /// </summary>
    /// <param name="context">The clamify database context to pull from.</param>
    public ExampleProvider(ClamifyContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public IEnumerable<Example> Get()
    {
        return _context.Examples;
    }
}
