using Amazon.SimpleEmailV2;
using Clamify.Core.Writers.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clamify.Core.Writers;

/// <summary>
/// Provides methods to send emails to users.
/// </summary>
public class EmailWriter : IMessageWriter
{
    private readonly ILogger<EmailWriter> _logger;
    private readonly IAmazonSimpleEmailServiceV2 _sesClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailWriter"/> class.
    /// </summary>
    /// <param name="logger">Logger object to record results/errors.</param>
    /// <param name="sesClient">The email client to send the email.</param>
    public EmailWriter(ILogger<EmailWriter> logger, IAmazonSimpleEmailServiceV2 sesClient)
    {
        _logger = logger;
        _sesClient = sesClient;
    }

    /// <inheritdoc/>
    public async Task SendMessage(
        string messageDestination,
        string userFirstName,
        string openingMessage,
        string buttonText = "Visit Clamify",
        string buttonLink = "",
        string closingText = "")
    {
        await Task.CompletedTask;
    }
}
