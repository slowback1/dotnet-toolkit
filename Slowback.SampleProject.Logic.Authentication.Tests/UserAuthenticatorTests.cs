using Slowback.Common;
using Slowback.Messaging;

namespace Slowback.SampleProject.Logic.Authentication.Tests;

public class UserAuthenticatorTests
{
    [SetUp]
    public void SetUpMessageBusEnvironment()
    {
        MessageBus.Publish(Messages.JwtAudience, "audience");
        MessageBus.Publish(Messages.JwtIssuer, "issuer");
        MessageBus.Publish(Messages.JwtSigningKey, "signing_key");
        MessageBus.Publish(Messages.JwtExpiration, TimeSpan.FromMinutes(1));
    }

    [Test]
    public void CreateToken_WhenCalledWithValidUserId_ReturnsAValidJwt()
    {
        var authenticator = new UserAuthenticator();

        var id = Guid.NewGuid().ToString();

        var token = authenticator.CreateToken(id);

        Assert.IsNotNull(token);

        var reader = new JwtReader(new JwtReaderOptions
        {
            Audience = "audience",
            Issuer = "issuer",
            SigningKey = "signing_key"
        });

        var result = reader.Read(token);

        Assert.IsTrue(result.IsValid);
        Assert.That(result.UserId, Is.EqualTo(id));
    }

    [Test]
    public void ValidateToken_ReturnsUserId_WhenTokenIsValid()
    {
        var authenticator = new UserAuthenticator();

        var id = Guid.NewGuid().ToString();

        var token = authenticator.CreateToken(id);

        var userId = authenticator.ValidateToken(token);

        Assert.That(userId, Is.EqualTo(id));
    }

    [Test]
    public void ValidateToken_ReturnsNull_WhenTokenIsInvalid()
    {
        var authenticator = new UserAuthenticator();

        var userId = authenticator.ValidateToken("invalid_token");

        Assert.IsNull(userId);
    }
}

public class UserAuthenticatorNoSetupTests
{
    [SetUp]
    public void SetUpMessageBusEnvironment()
    {
        MessageBus.ClearMessages();
    }

    [Test]
    public void ThrowsArgumentNullExceptionWhenJwtAudienceIsNull()
    {
        MessageBus.Publish(Messages.JwtIssuer, "issuer");
        MessageBus.Publish(Messages.JwtSigningKey, "sign");

        var authenticator = new UserAuthenticator();

        Assert.Throws<ArgumentNullException>(() => authenticator.CreateToken("id"));
    }

    [Test]
    public void ThrowsArgumentNullExceptionWhenJwtIssuerIsNull()
    {
        MessageBus.Publish(Messages.JwtAudience, "audience");
        MessageBus.Publish(Messages.JwtSigningKey, "sign");

        var authenticator = new UserAuthenticator();

        Assert.Throws<ArgumentNullException>(() => authenticator.CreateToken("id"));
    }

    [Test]
    public void ThrowsArgumentNullExceptionWhenJwtSigningKeyIsNull()
    {
        MessageBus.Publish(Messages.JwtAudience, "audience");
        MessageBus.Publish(Messages.JwtIssuer, "issuer");

        var authenticator = new UserAuthenticator();

        Assert.Throws<ArgumentNullException>(() => authenticator.CreateToken("id"));
    }

    [Test]
    public void ThrowsArgumentNullExceptionWhenJwtExpirationIsNull()
    {
        MessageBus.Publish(Messages.JwtAudience, "audience");
        MessageBus.Publish(Messages.JwtIssuer, "issuer");
        MessageBus.Publish(Messages.JwtSigningKey, "sign");

        var authenticator = new UserAuthenticator();

        Assert.Throws<ArgumentNullException>(() => authenticator.CreateToken("id"));
    }
}