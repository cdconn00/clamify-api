using Clamify.Web.Config;

/// <summary>
/// Entry point class for application.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point function for the application.
    /// </summary>
    /// <param name="args">Arguments to be supplied to the program.</param>
    public static void Main(string[] args)
    {
        var app = ClamifyWebApplicationBuilderProvider
            .Get(args)
            .Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHealthChecks("/health/live");
        app.UseCors("AllowAllPolicy");
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
