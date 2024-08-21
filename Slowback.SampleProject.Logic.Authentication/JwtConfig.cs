namespace Slowback.SampleProject.Logic.Authentication;

public class JwtConfig
{
    public string Issuer { get; set; }
    public string SigningKey { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
}