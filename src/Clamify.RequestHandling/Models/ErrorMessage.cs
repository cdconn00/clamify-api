namespace Clamify.RequestHandling.Models.Responses;

/// <summary>
/// Model to represent an error message.
/// </summary>
public class ErrorMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
    /// </summary>
    public ErrorMessage()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorMessage"/> class based on a message.
    /// </summary>
    /// <param name="messageText">Error message to use.</param>
    public ErrorMessage(string messageText)
    {
        ErrorCode = -100;
        FullMessage = messageText;
        ShortMessage = messageText;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorMessage"/> class based on an exception.
    /// </summary>
    /// <param name="e">The exception to use in creation.</param>
    public ErrorMessage(Exception e)
    {
        ErrorCode = -100;
        FullMessage = e.ToString();
        ShortMessage = e.Message;
    }

    /// <summary>
    /// Numerical error code.
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    /// Shortened version of error message.
    /// </summary>
    public string ShortMessage { get; set; } = string.Empty;

    /// <summary>
    /// Full error message.
    /// </summary>
    public string FullMessage { get; set; } = string.Empty;
}
