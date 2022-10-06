using Microsoft.EntityFrameworkCore;

namespace Clamify.Entities.Context;

/// <summary>
/// Represents the session utilized to query the database entities utilized by Clamify.
/// </summary>
public class ClamifyContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClamifyContext"/> class.
    /// </summary>
    /// <param name="options">The options to be utilized by ClamifyContext.</param>
    public ClamifyContext(DbContextOptions<ClamifyContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Example DbSet for project initialization and test verification.
    /// </summary>
    public virtual DbSet<Example> Examples { get; set; }

    /// <summary>
    /// Called after the derived context is intialized, allows configuration of models before the models are locked down and
    /// the context is initialized.
    /// </summary>
    /// <param name="modelBuilder">Object to construct models in the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Example.ConfigureModel(modelBuilder);
    }
}
