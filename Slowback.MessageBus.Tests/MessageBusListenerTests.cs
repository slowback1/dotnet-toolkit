namespace Slowback.MessageBus.Tests;

public class MessageBusListenerTests
{
    [OneTimeTearDown]
    public void TearDown()
    {
        MessageBus.GetInstance().ClearSubscribers();
        MessageBus.GetInstance().ClearMessages();
    }

    [Test]
    public async Task OnMessage_WhenCalled_SetsLastMessage()
    {
        var listener = new TestMessageBusListener();
        var message = "test_message";

        await listener.OnMessage(message);

        Assert.That(listener.LastMessage, Is.EqualTo(message));
    }

    [Test]
    public async Task ConstructingListensForMessageBusMessages()
    {
        var listener = new TestMessageBusListener();
        var message = "test_message";

        MessageBus.GetInstance().Publish("test_listener", message);

        Assert.That(listener.LastMessage, Is.EqualTo(message));
    }

    [Test]
    public async Task StaticInitializationListensForMessageBusMessages()
    {
        Assert.That(TestStaticMessageBusListener.Instance, Is.Not.Null);

        var message = 69;


        await MessageBus.GetInstance().PublishAsync("static_listener_test", message);

        Assert.That(TestStaticMessageBusListener.Instance.LastMessage, Is.EqualTo(message));
    }
}