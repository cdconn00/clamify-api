using Clamify.Core.Writers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;

namespace Clamify.Core.Writers;

/// <summary>
/// Implementation of provider to write example data to DB.
/// </summary>
public class ExampleWriter : IExampleWriter
{
    private readonly ClamifyContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleWriter"/> class.
    /// </summary>
    /// <param name="context">The database context to write to.</param>
    public ExampleWriter(ClamifyContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public void Write()
    {
        var examples = new[]
        {
            new Example(),
            new Example(),
            new Example(),
            new Example(),
            new Example(),
            new Example(),
        };

        _context.Examples.AddRange(examples);
        _context.SaveChanges();
    }
}
