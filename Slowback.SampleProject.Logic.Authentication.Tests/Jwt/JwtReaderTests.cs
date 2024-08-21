namespace Slowback.SampleProject.Logic.Authentication.Tests;

public class JwtReaderTests
{
    private readonly JwtReaderOptions _validOptions = new()
    {
        SigningKey = "mysecretkeymysecretkeymysecretkey",
        Issuer = "testIssuer",
        Audience = "testAudience"
    };

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenSigningKeyIsNull()
    {
        var options = new JwtReaderOptions
        {
            SigningKey = null,
            Issuer = "testIssuer",
            Audience = "testAudience"
        };

        Assert.Throws<ArgumentNullException>(() => new JwtReader(options));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenIssuerIsNull()
    {
        var options = new JwtReaderOptions
        {
            SigningKey = "mysecretkey",
            Issuer = null,
            Audience = "testAudience"
        };

        Assert.Throws<ArgumentNullException>(() => new JwtReader(options));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenAudienceIsNull()
    {
        var options = new JwtReaderOptions
        {
            SigningKey = "mysecretkey",
            Issuer = "testIssuer",
            Audience = null
        };

        Assert.Throws<ArgumentNullException>(() => new JwtReader(options));
    }

    [Test]
    public void Read_ShouldReturnValidResult_WhenJwtIsValid()
    {
        var jwtReader = new JwtReader(_validOptions);
        var validJwt = GenerateValidJwt();

        var result = jwtReader.Read(validJwt);

        Assert.IsTrue(result.IsValid);
        Assert.That(result.UserId, Is.EqualTo("user id"));
    }

    [Test]
    public void Read_ShouldReturnInvalidResult_WhenJwtIsInvalid()
    {
        var jwtReader = new JwtReader(_validOptions);
        var invalidJwt = "invalid.jwt.token";

        var result = jwtReader.Read(invalidJwt);

        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.UserId);
    }

    private string GenerateValidJwt()
    {
        var makerOptions = new JwtMakerOptions
        {
            Audience = _validOptions.Audience,
            Issuer = _validOptions.Issuer,
            SigningKey = _validOptions.SigningKey,
            Expiration = TimeSpan.FromHours(5)
        };

        var maker = new JwtMaker(makerOptions);

        return maker.MakeJwt("user id");
    }
}