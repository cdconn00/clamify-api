using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clamify.Entities;

/// <summary>
/// User entity extending <see cref="IdentityUser"/> with additional properties.
/// </summary>
public class ClamifyUser : IdentityUser<int>
{
    /// <summary>
    /// The schema name of the table in the DB.
    /// </summary>
    public const string SchemaName = "User";

    /// <summary>
    /// The name of the entity (without schema) in the DB.
    /// </summary>
    public const string TableName = "User";

    /// <summary>
    /// The given first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    ///  The given last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Function configuring properties, keys, and relationships for <see cref="ClamifyUser"/>.
    /// </summary>
    /// <param name="modelBuilder">Object that handles creation of entity.</param>
    public static void ConfigureModel(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ClamifyUser>(entity =>
        {
            entity.ToTable(TableName, SchemaName);

            entity.HasKey(x => x.Id);
            entity.Property(x => x.FirstName).HasColumnType("varchar(25)").IsRequired();
            entity.Property(x => x.LastName).HasColumnType("varchar(50)").IsRequired();
        });
}
