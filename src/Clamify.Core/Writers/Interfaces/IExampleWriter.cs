namespace Clamify.Core.Writers.Interfaces;

/// <summary>
/// Contract defining a method to write sample data to DB.
/// </summary>
public interface IExampleWriter
{
    /// <summary>
    /// Writes dummy example data to the DB.
    /// </summary>
    void Write();
}
