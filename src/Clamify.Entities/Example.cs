using Microsoft.EntityFrameworkCore;

namespace Clamify.Entities;

/// <summary>
/// Example entity representing a fictional table in the database, used to verify initial tests work properly.
/// </summary>
public class Example
{
    /// <summary>
    /// The name of the entity (without schema) in the DB.
    /// </summary>
    public const string TableName = "Example";

    /// <summary>
    /// The primary key of the entity.
    /// </summary>
    public int ExampleId { get; set; }

    /// <summary>
    /// Function configuring properties, keys, and relationships for Example.
    /// </summary>
    /// <param name="modelBuilder">Object that handles creation of entity.</param>
    public static void ConfigureModel(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Example>(entity =>
        {
            entity.ToTable(TableName, "Test");

            entity.HasKey(x => x.ExampleId);
            entity.Property(x => x.ExampleId).IsRequired();
        });
}
