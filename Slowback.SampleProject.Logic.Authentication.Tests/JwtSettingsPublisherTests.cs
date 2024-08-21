using Slowback.Common;
using Slowback.Messaging;

namespace Slowback.SampleProject.Logic.Authentication.Tests;

public class JwtSettingsPublisherTests
{
    [Test]
    public void PublishJwtSettings_WhenCalledWithNullConfig_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => JwtSettingsPublisher.PublishJwtSettings(null));
    }

    private JwtConfig GenerateValidJwtConfig()
    {
        return new JwtConfig
        {
            Audience = "testAudience",
            ExpirationMinutes = 10,
            Issuer = "testIssuer",
            SigningKey = "mysecretkeymysecretkeymysecretkey"
        };
    }

    [Test]
    public void PublishJwtSettings_WhenCalledWithValidConfig_DoesNotThrow()
    {
        var config = GenerateValidJwtConfig();
        Assert.DoesNotThrow(() => JwtSettingsPublisher.PublishJwtSettings(config));
    }

    [Test]
    public void PublishJwtSettings_WhenCalledWithValidConfig_PublishesIssuer()
    {
        var config = GenerateValidJwtConfig();
        JwtSettingsPublisher.PublishJwtSettings(config);

        var lastMessage = MessageBus.GetLastMessage<string>(Messages.JwtIssuer);

        Assert.That(lastMessage, Is.EqualTo(config.Issuer));
    }

    [Test]
    public void PublishJwtSettings_WhenCalledWithValidConfig_PublishesAudience()
    {
        var config = GenerateValidJwtConfig();
        JwtSettingsPublisher.PublishJwtSettings(config);

        var lastMessage = MessageBus.GetLastMessage<string>(Messages.JwtAudience);

        Assert.That(lastMessage, Is.EqualTo(config.Audience));
    }

    [Test]
    public void PublishJwtSettings_WhenCalledWithValidConfig_PublishesSigningKey()
    {
        var config = GenerateValidJwtConfig();
        JwtSettingsPublisher.PublishJwtSettings(config);

        var lastMessage = MessageBus.GetLastMessage<string>(Messages.JwtSigningKey);

        Assert.That(lastMessage, Is.EqualTo(config.SigningKey));
    }

    [Test]
    public void PublishJwtSettings_WhenCalledWithValidConfig_PublishesExpiration()
    {
        var config = GenerateValidJwtConfig();
        JwtSettingsPublisher.PublishJwtSettings(config);

        var lastMessage = MessageBus.GetLastMessage<TimeSpan>(Messages.JwtExpiration);

        Assert.That(lastMessage, Is.EqualTo(TimeSpan.FromMinutes(config.ExpirationMinutes)));
    }
}