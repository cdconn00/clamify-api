using System.Text.RegularExpressions;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Clamify.Core.Providers.Interfaces;
using Clamify.Core.Writers.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clamify.Core.Writers;

/// <summary>
/// Provides methods to send emails to users.
/// </summary>
public class EmailWriter : IMessageWriter
{
    private const string EmailFromAddress = "Clamify <pigeon@portofcall.clamify.org>";
    private const string DevSiteAddress = "https://dev.clamify.org";
    private const string ProductionSiteAddress = "https://clamify.org";
    private readonly bool _isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    private readonly ILogger<EmailWriter> _logger;
    private readonly IAmazonSimpleEmailServiceV2 _sesClient;
    private readonly ISecretProvider _secretProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailWriter"/> class.
    /// </summary>
    /// <param name="logger">Logger object to record results/errors.</param>
    /// <param name="sesClient">The email client to send the email.</param>
    /// <param name="secretProvider">Provider to get secrets.</param>
    public EmailWriter(ILogger<EmailWriter> logger, IAmazonSimpleEmailServiceV2 sesClient, ISecretProvider secretProvider)
    {
        _logger = logger;
        _sesClient = sesClient;
        _secretProvider = secretProvider;
    }

    /// <inheritdoc/>
    public async Task SendMessage(
        string messageDestination,
        string subject,
        string userFirstName,
        string openingMessage,
        string buttonText = "Visit Clamify",
        string buttonLink = "",
        string closingText = "")
    {
        if (string.IsNullOrEmpty(messageDestination))
        {
            _logger.LogError("Attempted to send an email without {arg}.", messageDestination);
            throw new ArgumentException($"{nameof(messageDestination)} was null or empty.");
        }

        if (string.IsNullOrEmpty(subject))
        {
            _logger.LogError("Attempted to send an email without {arg}.", subject);
            throw new ArgumentException($"{nameof(subject)} was null or empty.");
        }

        if (string.IsNullOrEmpty(userFirstName))
        {
            _logger.LogError("Attempted to send an email without {arg}.", userFirstName);
            throw new ArgumentException($"{nameof(userFirstName)} was null or empty.");
        }

        if (string.IsNullOrEmpty(openingMessage))
        {
            _logger.LogError("Attempted to send an email without {arg}.", openingMessage);
            throw new ArgumentException($"{nameof(openingMessage)} was null or empty.");
        }

        // If development environment, don't send to the actual user.
        if (_isDev)
        {
            messageDestination = GetDevEmail();
        }

        // Add domain URL to link.
        buttonLink = FormatLink(buttonLink);

        SendEmailRequest sendEmailRequest =
            GetEmailRequest(
                messageDestination,
                subject,
                BuildHtml(userFirstName, openingMessage, buttonText, buttonLink, closingText),
                BuildText(userFirstName, openingMessage, buttonText, buttonLink, closingText));

        await _sesClient.SendEmailAsync(sendEmailRequest);
    }

    private static SendEmailRequest GetEmailRequest(string emailTo, string subject, Content html, Content text)
    {
        return new SendEmailRequest
        {
            FromEmailAddress = EmailFromAddress,
            Destination = new Destination
            {
                ToAddresses = new List<string> { emailTo },
            },
            Content = new EmailContent
            {
                Simple = new Message
                {
                    Subject = new Content
                    {
                        Charset = "UTF-8",
                        Data = subject,
                    },
                    Body = new Body
                    {
                        Html = html,
                        Text = text,
                    },
                },
            },
        };
    }

    private static Content BuildHtml(
        string userFirstName,
        string openingMessage,
        string buttonText = "Visit Clamify",
        string buttonLink = "",
        string closingText = "")
    {
        string html = File.ReadAllText("Files/email_template.html");

        html = html.Replace("{name}", userFirstName);
        html = html.Replace("{opening_text}", openingMessage);
        html = html.Replace("{closing_text}", closingText);
        html = html.Replace("{button_text}", buttonText);
        html = html.Replace("{button_link}", buttonLink);

        html = html.Replace(Environment.NewLine, string.Empty);

        html = Regex.Unescape(html);

        return new Content
        {
            Charset = "UTF-8",
            Data = html,
        };
    }

    private static Content BuildText(
    string userFirstName,
    string openingMessage,
    string buttonText = "Visit Clamify",
    string buttonLink = "",
    string closingText = "")
    {
        return new Content
        {
            Charset = "UTF-8",
            Data = $"Hi, {userFirstName}.\n\n {openingMessage}\n\n {buttonText}: {buttonLink}\n\n {closingText}",
        };
    }

    private string FormatLink(string link)
    {
        string urlToUse = _isDev ? DevSiteAddress : ProductionSiteAddress;

        return urlToUse + link;
    }

    private string GetDevEmail()
    {
        return _secretProvider.GetSecret("DEV_EMAIL");
    }
}
