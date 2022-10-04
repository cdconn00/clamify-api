using Clamify.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Clamify.IntegrationTests.BaseHelpers;

/// <summary>
/// Context factory for getting a <see cref="ClamifyContext"/> object for use in integration tests.
/// </summary>
public class ClamifyContextFactory : IDesignTimeDbContextFactory<ClamifyContext>
{
    // To a new migration (updates the schema used by the integration test) run the following command in powershell from project root:
    // dotnet ef migrations add MigrationName --context ClamifyContext --startup-project ./tests/Clamify.IntegrationTests/Clamify.IntegrationTests.csproj --framework net6.0 --project ./tests/Clamify.IntegrationTests/Clamify.IntegrationTests.csproj

    /// <summary>
    /// Creates the database context based on provided arguments.
    /// </summary>
    /// <param name="args">Arguments to create context.</param>
    /// <returns>A created context.</returns>
    public ClamifyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ClamifyContext>();
        optionsBuilder.UseNpgsql(
            "Data Source=localhost",
            x => x.MigrationsAssembly("Clamify.IntegrationTests"));

        return new ClamifyContext(optionsBuilder.Options);
    }
}
