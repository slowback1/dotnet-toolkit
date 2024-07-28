using System.Net;

namespace Slowback.Mailer.Tests;

public class MockSmtpClient : ISmtpClient
{
    public List<System.Net.Mail.MailMessage> SentMessages { get; } = new();
    public ICredentialsByHost Credentials { get; set; }
    public bool EnableSsl { get; set; }

    public void Send(System.Net.Mail.MailMessage message)
    {
        SentMessages.Add(message);
    }

    public void Dispose()
    {
    }
}

[TestFixture]
public class EmailerTests
{
    [SetUp]
    public void SetUp()
    {
        _smtpClientMock = new MockSmtpClient();
        _settings = new EmailSettings
        {
            SmtpHost = "smtp.example.com",
            SmtpUsername = "username",
            SmtpPassword = "password",
            FromAddress = "from@example.com",
            FromName = "From Name"
        };
        _emailer = new Emailer(_settings, _smtpClientMock);
    }

    [TearDown]
    public void TearDown()
    {
        _smtpClientMock.Dispose();
    }

    private MockSmtpClient _smtpClientMock;
    private Emailer _emailer;
    private EmailSettings _settings;

    [Test]
    public void Send_ShouldSendEmail()
    {
        var message = new MailMessage
        {
            To = "to@example.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        _emailer.Send(message);

        Assert.That(_smtpClientMock.SentMessages.Count, Is.EqualTo(1));
    }

    [Test]
    public void Send_ShouldSetCredentialsAndEnableSsl()
    {
        var message = new MailMessage
        {
            To = "to@example.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        _emailer.Send(message);

        Assert.That(_smtpClientMock.Credentials, Is.Not.Null);
        Assert.That(_smtpClientMock.EnableSsl, Is.True);
    }

    [Test]
    public void Send_ShouldThrowException_WhenToAddressIsInvalid()
    {
        var message = new MailMessage
        {
            To = "invalid-email",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        Assert.Throws<FormatException>(() => _emailer.Send(message));
    }

    [Test]
    public void Send_ShouldSetCorrectSubjectAndBody()
    {
        var message = new MailMessage
        {
            To = "to@example.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        _emailer.Send(message);

        var sentMessage = _smtpClientMock.SentMessages.First();
        Assert.That(sentMessage.Subject, Is.EqualTo("Test Subject"));
        Assert.That(sentMessage.Body, Is.EqualTo("Test Body"));
    }
}