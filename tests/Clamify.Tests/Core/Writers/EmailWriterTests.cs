using Amazon.SimpleEmailV2;
using Clamify.Core.Writers;
using Clamify.Core.Writers.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Clamify.Tests.Core.Writers;

/// <summary>
/// Unit test class verifies <see cref="EmailWriter"/> methods.
/// </summary>
[TestClass]
public class EmailWriterTests
{
    private ILogger<EmailWriter> _logger;
    private IAmazonSimpleEmailServiceV2 _sesClient;

    private IMessageWriter GetWriter =>
        new EmailWriter(_logger, _sesClient);

    /// <summary>
    /// Initialize the tests with a mock context.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _logger = Mock.Of<ILogger<EmailWriter>>();
        _sesClient = Mock.Of<IAmazonSimpleEmailServiceV2>();
    }

    /// <summary>
    /// Unit test method verifies valid message parameters result in an email being sent.
    /// </summary>
    [TestMethod]
    public void SendMessage_GivenFullBody_SendsEmailSuccessfully()
    {
        string email = "validEmail@clamify.org";
        string subject = "Cool Subject";
        string fName = "Test";
        string openingMessage = "Opening Message";

        Action act = () =>
        {
            GetWriter.SendMessage(email, subject, fName, openingMessage).Wait();
        };

        act.Should().NotThrow();
    }

    /// <summary>
    /// Unit test method verifies empty email params result in an exception being thrown.
    /// </summary>
    [TestMethod]
    public void SendMessage_GivenEmptyEmail_ThrowsException()
    {
        string email = string.Empty;
        string subject = "Cool Subject";
        string fName = "Test";
        string openingMessage = "Opening Message";

        Action act = () =>
        {
            GetWriter.SendMessage(email, subject, fName, openingMessage).Wait();
        };

        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Unit test method verifies empty subject params result in an exception being thrown.
    /// </summary>
    [TestMethod]
    public void SendMessage_GivenEmptySubject_ThrowsException()
    {
        string email = "test";
        string subject = string.Empty;
        string fName = "Test";
        string openingMessage = "Opening Message";

        Action act = () =>
        {
            GetWriter.SendMessage(email, subject, fName, openingMessage).Wait();
        };

        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Unit test method verifies empty name params result in an exception being thrown.
    /// </summary>
    [TestMethod]
    public void SendMessage_GivenEmptyName_ThrowsException()
    {
        string email = "validEmail@clamify.org";
        string subject = "Cool Subject";
        string fName = string.Empty;
        string openingMessage = "Opening Message";

        Action act = () =>
        {
            GetWriter.SendMessage(email, subject, fName, openingMessage).Wait();
        };

        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Unit test method verifies empty message params result in an exception being thrown.
    /// </summary>
    [TestMethod]
    public void SendMessage_GivenEmptyMessage_ThrowsException()
    {
        string email = "validEmail@clamify.org";
        string subject = "Cool Subject";
        string fName = "Name";
        string openingMessage = string.Empty;

        Action act = () =>
        {
            GetWriter.SendMessage(email, subject, fName, openingMessage).Wait();
        };

        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Unit test method verifies no html file results in an exception being thrown.
    /// </summary>
    [TestMethod]
    public void SendMessage_GivenNoHtmlFile_ThrowsException()
    {
        string email = "validEmail@clamify.org";
        string subject = "Cool Subject";
        string fName = "Name";
        string openingMessage = "test";

        File.Move("Files/email_template.html", "Files/email_template-break.html");

        Action act = () =>
        {
            GetWriter.SendMessage(email, subject, fName, openingMessage).Wait();
        };

        act.Should().Throw<Exception>();

        File.Move("Files/email_template-break.html", "Files/email_template.html");
    }
}
