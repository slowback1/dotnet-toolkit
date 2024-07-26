namespace Slowback.MessageBus.Tests;

public class TestMessageBusListener : MessageBusListener<string>
{
    public TestMessageBusListener() : base("test_listener")
    {
    }

    public string LastMessage { get; private set; }

    public override async Task OnMessage(string message)
    {
        LastMessage = message;
    }
}