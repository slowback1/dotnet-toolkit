using Slowback.Common;
using Slowback.Messaging;
using Slowback.TestUtilities;

namespace Slowback.Mailer.Tests;

public class MailServiceTests
{
    private MailService _mailService { get; set; }
    private TestMailer _mailer { get; set; }

    [SetUp]
    public void SetUp()
    {
        _mailer = new TestMailer();
        _mailService = new MailService(_mailer);
    }

    [TearDown]
    public void TearDown()
    {
        MessageBus.ClearMessages();
        MessageBus.ClearSubscribers();
    }

    [Test]
    public async Task SendsMailWhenOnMessageIsCalled()
    {
        var message = new MailMessage
        {
            To = "To",
            Subject = "Subject",
            Body = "Body"
        };

        await _mailService.OnMessage(message);

        Assert.That(_mailer.LastTo, Is.EqualTo("To"));
        Assert.That(_mailer.LastSubject, Is.EqualTo("Subject"));
        Assert.That(_mailer.LastBody, Is.EqualTo("Body"));
    }

    [Test]
    public async Task SendsMailWhenCalledThroughTheMessageBus()
    {
        var message = new MailMessage
        {
            To = "To",
            Subject = "Subject",
            Body = "Body"
        };

        await MessageBus.PublishAsync(Messages.SendEmail, message);

        Assert.That(_mailer.LastTo, Is.EqualTo("To"));
        Assert.That(_mailer.LastSubject, Is.EqualTo("Subject"));
        Assert.That(_mailer.LastBody, Is.EqualTo("Body"));
    }

    [Test]
    public async Task SendMailRetriesThreeTimesWhenSmtpExceptionIsThrown()
    {
        var message = new MailMessage
        {
            To = "ERROR",
            Subject = "Subject",
            Body = "Body"
        };

        await _mailService.OnMessage(message);

        Assert.That(_mailer.TimesCalled, Is.EqualTo(3));
    }

    [Test]
    public async Task SendMailSendsAMessageToLogWhenSmtpIsThrownThreeTimes()
    {
        var message = new MailMessage
        {
            To = "ERROR",
            Subject = "Subject",
            Body = "Body"
        };

        await _mailService.OnMessage(message);

        var lastMessage = MessageBus.GetLastMessage<string>(Messages.LogMessage);

        Assert.That(lastMessage, Is.EqualTo("Failed to send email to ERROR after 3 attempts."));
    }

    [Test]
    public async Task SendMailDoesNotRetryWhenAnyOtherExceptionTypeIsThrown()
    {
        var message = new MailMessage
        {
            To = "To",
            Subject = "Subject",
            Body = null!
        };

        await _mailService.OnMessage(message);

        Assert.That(_mailer.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public async Task SendMailLogsErrorWhenAnyOtherExceptionTypeIsThrown()
    {
        var message = new MailMessage
        {
            To = "To",
            Subject = "Subject",
            Body = null!
        };

        await _mailService.OnMessage(message);

        var lastMessage = MessageBus.GetLastMessage<string>(Messages.LogMessage);

        Assert.That(lastMessage, Contains.Substring("Value does not fall within the expected range."));
    }
}