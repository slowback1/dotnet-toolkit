using Slowback.Common;
using Slowback.Logger.LoggingEngines;

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
}