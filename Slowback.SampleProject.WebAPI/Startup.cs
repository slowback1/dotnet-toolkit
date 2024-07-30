using Microsoft.IdentityModel.Protocols.Configuration;
using Slowback.Common;
using Slowback.Data.Core.EF;
using Slowback.Logger.LoggingEngines;
using Slowback.Messaging;

namespace Slowback.SampleProject.WebAPI;

public static class Startup
{
    public static void EnableLogging()
    {
        Logger.Logger.EnableLogging(new List<ILoggingEngine>
        {
            new ConsoleLoggingEngine(),
            new FileLoggingEngine(new FileLoggingEngineSettings
            {
                LogRotationStrategy = FileLogRotationStrategy.Monthly,
                TimestampFormat = FileLoggingTimestampFormat.Short
            })
        }, Messages.LogMessage);
    }

    public static void PublishAppDbConnection(ConfigurationManager configurationManager)
    {
        var section = configurationManager.GetSection("Database").Get<ConnectionOptions>();

        if (section is null)
            throw new InvalidConfigurationException("Database section is missing from configuration file.");

        MessageBus.Publish(Messages.AppDbConnection, section);
    }
}