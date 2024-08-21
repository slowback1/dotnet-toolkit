using System;
using Slowback.Common;
using Slowback.Messaging;

namespace Slowback.SampleProject.Logic.Authentication;

public static class JwtSettingsPublisher
{
    public static void PublishJwtSettings(JwtConfig? config)
    {
        if (config is null) throw new ArgumentNullException();

        MessageBus.Publish(Messages.JwtIssuer, config.Issuer);
        MessageBus.Publish(Messages.JwtAudience, config.Audience);
        MessageBus.Publish(Messages.JwtSigningKey, config.SigningKey);
        MessageBus.Publish(Messages.JwtExpiration, TimeSpan.FromMinutes(config.ExpirationMinutes));
    }
}