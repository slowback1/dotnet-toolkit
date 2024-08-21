using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Slowback.SampleProject.Logic.Authentication;

internal struct JwtReaderOptions
{
    internal string SigningKey { get; set; }
    internal string Issuer { get; set; }
    internal string Audience { get; set; }
}

internal struct JwtReadResult
{
    internal string UserId { get; set; }
    internal bool IsValid { get; set; }
}

internal class JwtReader : Jwt
{
    public JwtReader(JwtReaderOptions options)
    {
        if (options.SigningKey == null) throw new ArgumentNullException(nameof(options.SigningKey));
        if (options.Issuer == null) throw new ArgumentNullException(nameof(options.Issuer));
        if (options.Audience == null) throw new ArgumentNullException(nameof(options.Audience));

        options.SigningKey = LeftPadSigningKey(options.SigningKey);

        Options = options;
    }

    private JwtReaderOptions Options { get; }

    internal JwtReadResult Read(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Options.Issuer,
            ValidAudience = Options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Options.SigningKey))
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, validationParameters, out _);
            var userId = claimsPrincipal.FindFirst(UserIdClaimType)?.Value;

            return new JwtReadResult
            {
                UserId = userId,
                IsValid = true
            };
        }
        catch (Exception e)
        {
            return new JwtReadResult
            {
                IsValid = false
            };
        }
    }
}