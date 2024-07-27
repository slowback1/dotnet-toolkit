using Slowback.Logger.LoggingEngines;
using Slowback.Messaging;

namespace Slowback.Logger.Tests;

public class LoggerTests
{
    private TestLoggingEngine TestLoggingEngine { get; set; }

    [SetUp]
    public void Setup()
    {
        TestLoggingEngine = new TestLoggingEngine();
        Logger.EnableLogging(new List<ILoggingEngine> { TestLoggingEngine });
    }

    [TearDown]
    public void TearDown()
    {
        Logger.DisableLogging();
    }

    [Test]
    public void WillLogAMessageFromTheMessageBusThatImplementsILoggable()
    {
        var testLoggingObject = new TestLoggingObject("Test");
        MessageBus.Publish("whatever", testLoggingObject);
        Assert.That(TestLoggingEngine.Messages.Count, Is.EqualTo(1));
        Assert.That(TestLoggingEngine.Messages[0], Is.EqualTo("Name: Test"));
    }

    [Test]
    public void WillNotLogAMessageFromTheMessageBusThatDoesNotImplementILoggable()
    {
        MessageBus.Publish("whatever", "Test");
        Assert.That(TestLoggingEngine.Messages.Count, Is.EqualTo(0));
    }

    [Test]
    public void WillLogAStringMessageFromTheMessageBusForTheLoggingMessage()
    {
        MessageBus.Publish(Logger.DefaultLoggingMessage, "Test");
        Assert.That(TestLoggingEngine.Messages.Count, Is.EqualTo(1));
        Assert.That(TestLoggingEngine.Messages[0], Is.EqualTo("Test"));
    }

    [Test]
    public void CanCustomizeTheMessageThatListensForStringMessages()
    {
        var customLoggingMessage = "CUSTOMLOG";
        Logger.DisableLogging();
        Logger.EnableLogging(new List<ILoggingEngine> { TestLoggingEngine }, customLoggingMessage);
        MessageBus.Publish(customLoggingMessage, "Test");
        Assert.That(TestLoggingEngine.Messages.Count, Is.EqualTo(1));
        Assert.That(TestLoggingEngine.Messages[0], Is.EqualTo("Test"));
    }

    [Test]
    public void CanLogFromMultipleEnginesAtTheSameTime()
    {
        var testLoggingEngine2 = new TestLoggingEngine();
        Logger.DisableLogging();
        Logger.EnableLogging(new List<ILoggingEngine> { TestLoggingEngine, testLoggingEngine2 });
        MessageBus.Publish(Logger.DefaultLoggingMessage, "Test");
        Assert.That(TestLoggingEngine.Messages.Count, Is.EqualTo(1));
        Assert.That(TestLoggingEngine.Messages[0], Is.EqualTo("Test"));
        Assert.That(testLoggingEngine2.Messages.Count, Is.EqualTo(1));
        Assert.That(testLoggingEngine2.Messages[0], Is.EqualTo("Test"));
    }
}