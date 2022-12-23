using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clamify.Entities.Context;

/// <summary>
/// Represents the session utilized to query the database entities utilized by Clamify.
/// </summary>
public class ClamifyContext : IdentityDbContext<ClamifyUser, IdentityRole<int>, int>
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
        base.OnModelCreating(modelBuilder);

        ConfigureIdentityModels(modelBuilder);

        Example.ConfigureModel(modelBuilder);
    }

    private void ConfigureIdentityModels(ModelBuilder modelBuilder)
    {
        string schema = ClamifyUser.SchemaName;

        modelBuilder.Entity<ClamifyUser>(entity =>
        {
            entity.ToTable(name: "User", schema: schema);
        });

        modelBuilder.Entity<IdentityRole<int>>(entity =>
        {
            entity.ToTable(name: "Role", schema: schema);
        });

        modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
        {
            entity.ToTable("UserClaim", schema);
        });

        modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("UserLogin", schema);
        });

        modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
        {
            entity.ToTable("RoleClaim", schema);
        });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("UserRole", schema);
        });

        modelBuilder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("UserToken", schema);
        });
    }
}
