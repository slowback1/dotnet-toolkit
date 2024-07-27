using Slowback.Common;
using Slowback.Logger.LoggingEngines;
using TimeProvider = Slowback.Time.TimeProvider;

namespace Slowback.WebAPI;

public static class Startup
{
    public static void EnableLogging()
    {
        Logger.Logger.EnableLogging(new List<ILoggingEngine>
        {
            new ConsoleLoggingEngine(),
            new FileLoggingEngine(new TimeProvider(), new FileLoggingEngineSettings
            {
                LogRotationStrategy = FileLogRotationStrategy.Monthly,
                TimestampFormat = FileLoggingTimestampFormat.Short
            })
        }, Messages.LogMessage);
    }
}