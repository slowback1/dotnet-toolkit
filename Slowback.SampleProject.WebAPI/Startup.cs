using Slowback.Common;
using Slowback.Logger.LoggingEngines;
using Slowback.Time;

namespace Slowback.SampleProject.WebAPI;

public static class Startup
{
    public static void EnableLogging()
    {
        Logger.Logger.EnableLogging(new List<ILoggingEngine>
        {
            new ConsoleLoggingEngine(),
            new FileLoggingEngine(TimeEnvironment.Provider, new FileLoggingEngineSettings
            {
                LogRotationStrategy = FileLogRotationStrategy.Monthly,
                TimestampFormat = FileLoggingTimestampFormat.Short
            })
        }, Messages.LogMessage);
    }
}