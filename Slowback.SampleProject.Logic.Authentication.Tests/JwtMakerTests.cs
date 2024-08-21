using System.IdentityModel.Tokens.Jwt;

namespace Slowback.SampleProject.Logic.Authentication.Tests;

public class JwtMakerTests
{
    [Test]
    public void JwtMaker_WhenConstructedWithNullOption_ThrowsArgumentExceptionForAudience()
    {
        var options = new JwtMakerOptions
        {
            Expiration = TimeSpan.FromMinutes(1),
            Issuer = "issuer",
            SigningKey = "signingKey"
        };
        Assert.Throws<ArgumentNullException>(() => new JwtMaker(options));
    }

    [Test]
    public void JwtMaker_WhenConstructedWithNullOption_ThrowsArgumentExceptionForExpiration()
    {
        var options = new JwtMakerOptions
        {
            Audience = "audience",
            Issuer = "issuer",
            SigningKey = "signingKey"
        };
        Assert.Throws<ArgumentNullException>(() => new JwtMaker(options));
    }

    [Test]
    public void JwtMaker_WhenConstructedWithNullOption_ThrowsArgumentExceptionForIssuer()
    {
        var options = new JwtMakerOptions
        {
            Audience = "audience",
            Expiration = TimeSpan.FromMinutes(1),
            SigningKey = "signingKey"
        };
        Assert.Throws<ArgumentNullException>(() => new JwtMaker(options));
    }

    [Test]
    public void JwtMaker_WhenConstructedWithNullOption_ThrowsArgumentExceptionForSigningKey()
    {
        var options = new JwtMakerOptions
        {
            Audience = "audience",
            Expiration = TimeSpan.FromMinutes(1),
            Issuer = "issuer"
        };
        Assert.Throws<ArgumentNullException>(() => new JwtMaker(options));
    }

    [Test]
    public void MakeJwt_ReturnsAValidJwt()
    {
        var options = new JwtMakerOptions
        {
            Audience = "audience",
            Expiration = TimeSpan.FromMinutes(1),
            Issuer = "issuer",
            SigningKey = "really_really_really_really_large_key"
        };

        var jwtMaker = new JwtMaker(options);

        var jwt = jwtMaker.MakeJwt("id");

        Assert.IsNotNull(jwt);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(jwt);

        Assert.That(token.Audiences.First(), Is.EqualTo(options.Audience));
        Assert.That(token.Issuer, Is.EqualTo(options.Issuer));
        Assert.That(token.ValidTo - token.ValidFrom, Is.EqualTo(options.Expiration));
    }

    [Test]
    public void LeftPadsTheSigningKeyIfItIsShort()
    {
        var options = new JwtMakerOptions
        {
            Audience = "audience",
            Expiration = TimeSpan.FromMinutes(1),
            Issuer = "issuer",
            SigningKey = "short"
        };

        var jwtMaker = new JwtMaker(options);

        var jwt = jwtMaker.MakeJwt("id");

        Assert.IsNotNull(jwt);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(jwt);

        Assert.That(token.Audiences.First(), Is.EqualTo(options.Audience));
        Assert.That(token.Issuer, Is.EqualTo(options.Issuer));
        Assert.That(token.ValidTo - token.ValidFrom, Is.EqualTo(options.Expiration));
    }

    [Test]
    public void IncludesTheUserIdAsAClaim()
    {
        var options = new JwtMakerOptions
        {
            Audience = "audience",
            Expiration = TimeSpan.FromMinutes(1),
            Issuer = "issuer",
            SigningKey = "really_really_really_really_large_key"
        };

        var jwtMaker = new JwtMaker(options);

        var jwt = jwtMaker.MakeJwt("my cool id");

        Assert.IsNotNull(jwt);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(jwt);

        Assert.That(token.Claims.First().Type, Is.EqualTo("userId"));
        Assert.That(token.Claims.First().Value, Is.EqualTo("my cool id"));
    }
}