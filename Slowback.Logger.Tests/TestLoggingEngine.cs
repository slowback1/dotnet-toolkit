using Slowback.Logger.LoggingEngines;

namespace Slowback.Logger.Tests;

public class TestLoggingEngine : ILoggingEngine
{
    public List<string> Messages { get; } = new();

    public void Log(string message)
    {
        Messages.Add(message);
    }
}