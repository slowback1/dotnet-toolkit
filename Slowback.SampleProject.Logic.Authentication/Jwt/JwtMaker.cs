using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Slowback.SampleProject.Logic.Authentication;

internal class JwtMakerOptions
{
    internal string SigningKey { get; set; }
    internal string Issuer { get; set; }
    internal string Audience { get; set; }
    internal TimeSpan Expiration { get; set; }
}

internal class JwtMaker : Jwt
{
    public JwtMaker(JwtMakerOptions options)
    {
        if (options.SigningKey == null) throw new ArgumentNullException(nameof(options.SigningKey));
        if (options.Issuer == null) throw new ArgumentNullException(nameof(options.Issuer));
        if (options.Audience == null) throw new ArgumentNullException(nameof(options.Audience));
        if (options.Expiration == null || options.Expiration == new TimeSpan())
            throw new ArgumentNullException(nameof(options.Expiration));

        options.SigningKey = LeftPadSigningKey(options.SigningKey);

        Options = options;
    }

    private JwtMakerOptions Options { get; }

    public string MakeJwt(string userId)
    {
        var tokenDescriptor = BuildToken(userId);

        return WriteToken(tokenDescriptor);
    }

    private SecurityTokenDescriptor BuildToken(string userId)
    {
        var key = Encoding.ASCII.GetBytes(Options.SigningKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(UserIdClaimType, userId)
            }),
            Expires = DateTime.UtcNow.Add(Options.Expiration),
            Issuer = Options.Issuer,
            Audience = Options.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenDescriptor;
    }

    private static string WriteToken(SecurityTokenDescriptor tokenDescriptor)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}