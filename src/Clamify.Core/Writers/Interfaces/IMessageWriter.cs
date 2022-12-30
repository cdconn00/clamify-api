namespace Clamify.Core.Writers.Interfaces;

/// <summary>
/// Contract defining methods to send a message to a user.
/// </summary>
public interface IMessageWriter
{
    /// <summary>
    /// Sends a created message to a user.
    /// </summary>
    /// <param name="messageDestination">Specifies the address the message will be sent to / usually a phone number or email.</param>
    /// <param name="subject">The subject of the message.</param>
    /// <param name="userFirstName">The first name of the user the message is sent to.</param>
    /// <param name="openingMessage">The opening text of the message.</param>
    /// <param name="buttonText">The label for the button. If left empty it will default to 'Visit Clamify'.</param>
    /// <param name="buttonLink">The link the button will go to. If left empty it will default to the clamify homepage.</param>
    /// <param name="closingText">The text (if any) to put after the button.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task SendMessage(
        string messageDestination,
        string subject,
        string userFirstName,
        string openingMessage,
        string buttonText = "Visit Clamify",
        string buttonLink = "",
        string closingText = "");
}
