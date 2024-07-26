namespace Slowback.Logger.LoggingEngines;

public class ConsoleLoggingEngine : ILoggingEngine
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}