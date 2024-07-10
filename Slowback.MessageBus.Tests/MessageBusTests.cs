namespace Slowback.MessageBus.Tests;

public class TestMessage
{
    public string Message { get; set; }
    public int Value { get; set; }
}

public class MessageBusTests
{
    [TearDown]
    public void ClearSubscribers()
    {
        MessageBus.GetInstance().ClearSubscribers();
    }

    [Test]
    public void MessageBusIsASingleton()
    {
        var messageBus1 = MessageBus.GetInstance();
        var messageBus2 = MessageBus.GetInstance();

        Assert.That(messageBus2, Is.SameAs(messageBus1));
    }

    [Test]
    public void CanSubscribeToAMessageAndReceiveIt()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage = "";

        messageBus.Subscribe<string>(message, m => receivedMessage = m);
        messageBus.Publish(message, message);

        Assert.That(receivedMessage, Is.SameAs(message));
    }

    [Test]
    public void CanHandleMultipleSubscribersToTheSameMessage()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage1 = "";
        var receivedMessage2 = "";

        messageBus.Subscribe<string>(message, m => receivedMessage1 = m);
        messageBus.Subscribe<string>(message, m => receivedMessage2 = m);
        messageBus.Publish(message, message);

        Assert.That(receivedMessage1, Is.SameAs(message));
        Assert.That(receivedMessage2, Is.SameAs(message));
    }

    [Test]
    public void DoesntBreakWhenPublishingToAnUnsubscribedMessage()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "i_have_no_subscribers";
        var receivedMessage = "";

        messageBus.Publish(message, message);

        Assert.That(receivedMessage, Is.Empty);
    }

    [Test]
    public async Task CanHandleAnAsyncAction()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage = "";

        messageBus.Subscribe<string>(message, async m =>
        {
            await Task.Delay(100);
            receivedMessage = m;
        });

        messageBus.Publish(message, message);

        await Task.Delay(200);

        Assert.That(receivedMessage, Is.SameAs(message));
    }

    [Test]
    public async Task PublishHasAFunctioningAsyncOverride()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage = "";

        messageBus.Subscribe<string>(message, async m => { receivedMessage = m; });

        await messageBus.PublishAsync(message, message);

        Assert.That(receivedMessage, Is.SameAs(message));
    }

    [Test]
    public void CanHandleComplexTypes()
    {
        var messageBus = MessageBus.GetInstance();
        var message = new TestMessage { Message = "complex", Value = 42 };
        TestMessage receivedMessage = null;

        messageBus.Subscribe<TestMessage>(message.Message, m => receivedMessage = m);
        messageBus.Publish(message.Message, message);

        Assert.That(receivedMessage, Is.SameAs(message));
    }

    [Test]
    public void CanClearSubscribers()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage = "";

        messageBus.Subscribe<string>(message, m => receivedMessage = m);
        messageBus.ClearSubscribers();

        messageBus.Publish(message, message);

        Assert.That(receivedMessage, Is.Empty);
    }

    [Test]
    public void CanUnsubscribe()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage = "";

        var unsubscribe = messageBus.Subscribe<string>(message, m => receivedMessage = m);

        unsubscribe();

        messageBus.Publish(message, "hello world");

        Assert.That(receivedMessage, Is.Empty);
    }

    [Test]
    [Repeat(1000)]
    public void UnsubscribingDoesNotAlsoUnsubscribeOtherSubscriptions()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage1 = "";
        var receivedMessage2 = "";

        var unsubscribe1 = messageBus.Subscribe<string>(message, m => receivedMessage1 = m);
        messageBus.Subscribe<string>(message, m => receivedMessage2 = m);

        unsubscribe1();

        messageBus.Publish(message, "hello world");

        Assert.That(receivedMessage1, Is.Empty);
        Assert.That(receivedMessage2, Is.SameAs("hello world"));
    }

    [Test]
    public void DoesExplodeWhenSubscribingToTheSameMessageWithMultipleDifferentTypes()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";
        var receivedMessage1 = "";
        var receivedMessage2 = "";

        messageBus.Subscribe<string>(message, m => receivedMessage1 = m);
        messageBus.Subscribe<int>(message, m => receivedMessage2 = m.ToString());

        Assert.Throws<InvalidCastException>(() => messageBus.Publish(message, 42));
    }

    [Test]
    public void CanGetLastMessage()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";

        messageBus.Publish(message, "hello world");

        Assert.That(messageBus.GetLastMessage<string>(message), Is.SameAs("hello world"));
    }

    [Test]
    public async Task CanGetLastMessageAsync()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";

        await messageBus.PublishAsync(message, "hello world2");

        Assert.That(messageBus.GetLastMessage<string>(message), Is.SameAs("hello world2"));
    }

    [Test]
    public void CanClearMessageBusMessages()
    {
        var messageBus = MessageBus.GetInstance();
        var message = "test";

        messageBus.Publish(message, "hello world");
        messageBus.ClearMessages();

        Assert.That(messageBus.GetLastMessage<string>(message), Is.Null);
    }
}