using Slowback.Logger.LoggingEngines;

namespace Slowback.Logger.Tests.LoggingEngines;

public class ConsoleLoggingEngineTests
{
    [Test]
    public void ConsoleLoggingEngineLogsMessageToConsole()
    {
        using var sw = new StringWriter();
        Console.SetOut(sw);

        var consoleLoggingEngine = new ConsoleLoggingEngine();
        consoleLoggingEngine.Log("Test");

        Assert.That(sw.ToString(), Is.EqualTo("Test" + Environment.NewLine));
    }
}