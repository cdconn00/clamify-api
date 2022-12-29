using Microsoft.EntityFrameworkCore;

namespace Clamify.Entities;

/// <summary>
/// Entity representing if a feature is flagged as enabled or disabled.
/// </summary>
public class FeatureFlag
{
    /// <summary>
    /// The schema name for this entity.
    /// </summary>
    public const string SchemaName = "Config";

    /// <summary>
    /// The name of the entity (without schema) in the DB.
    /// </summary>
    public const string TableName = "FeatureFlag";

    /// <summary>
    /// The primary key of the entity.
    /// </summary>
    public int FeatureFlagId { get; set; }

    /// <summary>
    /// The name of the feature.
    /// </summary>
    public string FeatureName { get; set; }

    /// <summary>
    /// If the feature is enabled or not.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Function configuring properties, keys, and relationships for Example.
    /// </summary>
    /// <param name="modelBuilder">Object that handles creation of entity.</param>
    public static void ConfigureModel(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<FeatureFlag>(entity =>
        {
            entity.ToTable(TableName, SchemaName);

            entity.HasKey(x => x.FeatureFlagId);

            entity.Property(x => x.FeatureFlagId).IsRequired();
            entity.Property(x => x.FeatureName).HasColumnType("varchar(50)").IsRequired();
            entity.Property(x => x.IsEnabled).IsRequired();
        });
}
