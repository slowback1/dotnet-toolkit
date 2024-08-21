using System;
using Slowback.Common;
using Slowback.Messaging;

namespace Slowback.SampleProject.Logic.Authentication;

public class UserAuthenticator
{
    public string CreateToken(string userId)
    {
        var options = GetJwtMakerOptions();

        var maker = new JwtMaker(options);

        return maker.MakeJwt(userId);
    }

    public string? ValidateToken(string token)
    {
        var options = GetJwtReaderOptions();

        var reader = new JwtReader(options);

        var result = reader.Read(token);

        return result.IsValid ? result.UserId : null;
    }

    private JwtMakerOptions GetJwtMakerOptions()
    {
        return new JwtMakerOptions
        {
            Audience = MessageBus.GetLastMessage<string>(Messages.JwtAudience),
            Expiration = MessageBus.GetLastMessage<TimeSpan>(Messages.JwtExpiration),
            Issuer = MessageBus.GetLastMessage<string>(Messages.JwtIssuer),
            SigningKey = MessageBus.GetLastMessage<string>(Messages.JwtSigningKey)
        };
    }

    private JwtReaderOptions GetJwtReaderOptions()
    {
        return new JwtReaderOptions
        {
            Audience = MessageBus.GetLastMessage<string>(Messages.JwtAudience),
            Issuer = MessageBus.GetLastMessage<string>(Messages.JwtIssuer),
            SigningKey = MessageBus.GetLastMessage<string>(Messages.JwtSigningKey)
        };
    }
}