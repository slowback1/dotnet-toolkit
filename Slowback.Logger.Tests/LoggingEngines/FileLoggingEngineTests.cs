using Slowback.Logger.LoggingEngines;

namespace Slowback.Logger.Tests.LoggingEngines;

public class FileLoggingEngineTests
{
    [TearDown]
    public void TearDown()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        if (File.Exists(filePath)) File.Delete(filePath);
    }


    [Test]
    public void FileLoggingEngineLogsMessageToFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        var fileLoggingEngine = new FileLoggingEngine();
        fileLoggingEngine.Log("Test");

        Assert.That(File.ReadAllText(filePath), Is.EqualTo("Test" + Environment.NewLine));
    }

    [Test]
    public void FileLoggingEngineAppendsMessageToFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        var fileLoggingEngine = new FileLoggingEngine();
        fileLoggingEngine.Log("Test");
        fileLoggingEngine.Log("Test2");

        Assert.That(File.ReadAllText(filePath),
            Is.EqualTo("Test" + Environment.NewLine + "Test2" + Environment.NewLine));
    }

    [Test]
    public void FileLoggingEngineCanSupportPrependingWithAShortTimestamp()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        var fileLoggingEngine = new FileLoggingEngine(new FileLoggingEngineSettings
        {
            TimestampFormat = FileLoggingTimestampFormat.Short
        });
        fileLoggingEngine.Log("Test");

        var logFileContents = File.ReadAllText(filePath);
        var logFileLines = logFileContents.Split(Environment.NewLine);
        Assert.That(logFileLines.Length, Is.EqualTo(2));
        Assert.That(logFileLines[0], Does.Match(@"\(\d{2}:\d{2}:\d{2}\) Test"));
    }

    [Test]
    public void FileLoggingEngineCanSupportPrependingWithALongTimestamp()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        var fileLoggingEngine = new FileLoggingEngine(new FileLoggingEngineSettings
        {
            TimestampFormat = FileLoggingTimestampFormat.Long
        });
        fileLoggingEngine.Log("Test");

        var logFileContents = File.ReadAllText(filePath);
        var logFileLines = logFileContents.Split(Environment.NewLine);
        Assert.That(logFileLines.Length, Is.EqualTo(2));
        Assert.That(logFileLines[0], Does.Match(@"\(\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}\.\d{3}\) Test"));
    }
}