using Slowback.Logger.LoggingEngines;
using Slowback.TestUtilities;
using Slowback.Time;

namespace Slowback.Logger.Tests.LoggingEngines;

public class FileLoggingEngineTests
{
    private static readonly string FileName = "2021-01-01.log";
    private static ITimeProvider TimeProvider => new TestTimeProvider(new DateTime(2021, 1, 1));

    [SetUp]
    public void SetUp()
    {
        TimeEnvironment.SetProvider(TimeProvider);
    }

    [TearDown]
    public void TearDown()
    {
        var filePath = Path.Combine(Path.GetTempPath(), FileName);
        if (File.Exists(filePath)) File.Delete(filePath);
    }


    [Test]
    public void FileLoggingEngineLogsMessageToFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), FileName);
        var fileLoggingEngine = new FileLoggingEngine();
        fileLoggingEngine.Log("Test");

        Assert.That(File.ReadAllText(filePath), Is.EqualTo("Test" + Environment.NewLine));
    }

    [Test]
    public void FileLoggingEngineAppendsMessageToFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), FileName);
        var fileLoggingEngine = new FileLoggingEngine();
        fileLoggingEngine.Log("Test");
        fileLoggingEngine.Log("Test2");

        Assert.That(File.ReadAllText(filePath),
            Is.EqualTo("Test" + Environment.NewLine + "Test2" + Environment.NewLine));
    }

    [Test]
    public void FileLoggingEngineCanSupportPrependingWithAShortTimestamp()
    {
        var filePath = Path.Combine(Path.GetTempPath(), FileName);
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
        var filePath = Path.Combine(Path.GetTempPath(), FileName);
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

    [Test]
    public void FileLoggingEngineChangesTheFileNameWhenTheDateChanges()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "2021-01-02.log");

        if (File.Exists(filePath)) File.Delete(filePath);

        var otherTimeProvider = new TestTimeProvider(new DateTime(2021, 1, 2));
        TimeEnvironment.SetProvider(otherTimeProvider);
        var fileLoggingEngine = new FileLoggingEngine();
        fileLoggingEngine.Log("Test");

        var logFileContents = File.ReadAllText(filePath);
        var logFileLines = logFileContents.Split(Environment.NewLine);
        Assert.That(logFileLines.Length, Is.EqualTo(2));
        Assert.That(logFileLines[0], Is.EqualTo("Test"));
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    public void DoesNotChangeFileNamesGoingFromNDaysIfLogRotationStrategyIsWeekly(int daysAfterMonday)
    {
        var filePath = Path.Combine(Path.GetTempPath(), "2024-07-22.log");

        if (File.Exists(filePath)) File.Delete(filePath);

        var timeProvider = new TestTimeProvider(new DateTime(2024, 7, 22 + daysAfterMonday));

        TimeEnvironment.SetProvider(timeProvider);

        var fileLoggingEngine =
            new FileLoggingEngine(new FileLoggingEngineSettings
            {
                LogRotationStrategy = FileLogRotationStrategy.Weekly
            });
        fileLoggingEngine.Log("Test");

        var logFileContents = File.ReadAllText(filePath);
        var logFileLines = logFileContents.Split(Environment.NewLine);
        Assert.That(logFileLines.Length, Is.EqualTo(2));
        Assert.That(logFileLines[0], Is.EqualTo("Test"));
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    [TestCase(16)]
    [TestCase(17)]
    [TestCase(18)]
    [TestCase(19)]
    [TestCase(20)]
    [TestCase(21)]
    [TestCase(22)]
    [TestCase(23)]
    [TestCase(24)]
    [TestCase(25)]
    [TestCase(26)]
    [TestCase(27)]
    [TestCase(28)]
    [TestCase(29)]
    [TestCase(30)]
    [TestCase(31)]
    public void KeepsTheFileNameAsTheFirstDayOfTheMonthWhenTheLogRotationStrategyIsMonthly(int dayOfTheMonth)
    {
        var filePath = Path.Combine(Path.GetTempPath(), "2024-07-01.log");

        if (File.Exists(filePath)) File.Delete(filePath);

        var timeProvider = new TestTimeProvider(new DateTime(2024, 7, dayOfTheMonth));
        TimeEnvironment.SetProvider(timeProvider);

        var fileLoggingEngine =
            new FileLoggingEngine(new FileLoggingEngineSettings
            {
                LogRotationStrategy = FileLogRotationStrategy.Monthly
            });
        fileLoggingEngine.Log("Test");

        var logFileContents = File.ReadAllText(filePath);
        var logFileLines = logFileContents.Split(Environment.NewLine);
        Assert.That(logFileLines.Length, Is.EqualTo(2));
        Assert.That(logFileLines[0], Is.EqualTo("Test"));
    }
}