using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Clamify.RequestHandling.Models.Responses;

/// <summary>
/// Provides basis for responses without helper methods and members.
/// </summary>
public abstract class ResponseMessageBase
{
    /// <summary>
    /// A list of errors the response is returning with.
    /// </summary>
    [DataMember]
    public IList<ErrorMessage> Errors { get; set; } = new List<ErrorMessage>();

    /// <summary>
    /// Returns if the response contains any errors.
    /// </summary>
    [JsonIgnore]
    public bool ContainsErrors => Errors?.Any() ?? false;

    /// <summary>
    /// Returns a stringified combination of all error messages.
    /// </summary>
    [JsonIgnore]
    public string CombinedErrorMessages => Errors != null ? string.Join(",", Errors.Select(x => x.ShortMessage)) : "";

    /// <summary>
    /// Adds an error to the list with a specific message.
    /// </summary>
    /// <param name="messageText">The error message to add to the error.</param>
    public void AddErrorMessage(string messageText)
    {
        var message = new ErrorMessage(messageText);
        AddErrorMessage(message);
    }

    /// <summary>
    /// Adds an error to the list with a specific error message.
    /// </summary>
    /// <param name="errorMessage">The error message object to add to the list.</param>
    public void AddErrorMessage(ErrorMessage errorMessage)
    {
        Errors.Add(errorMessage);
    }

    /// <summary>
    /// Adds an error to the list based on an exception.
    /// </summary>
    /// <param name="e">The exception to add as an error to the list.</param>
    public void AddErrorMessage(Exception e)
    {
        var msg = new ErrorMessage(e);
        AddErrorMessage(msg);
    }
}
