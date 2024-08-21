namespace Slowback.SampleProject.Logic.Authentication;

public abstract class Jwt
{
    protected const string UserIdClaimType = "UserId";

    protected string LeftPadSigningKey(string signingKey)
    {
        if (signingKey.Length < 32) return signingKey.PadLeft(32, '0');

        return signingKey;
    }
}