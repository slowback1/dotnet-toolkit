namespace Slowback.Logger.Tests;

public class TestLoggingObject : ILoggable
{
    public TestLoggingObject(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public string ToLoggableString()
    {
        return $"Name: {Name}";
    }
}