namespace Slowback.Data.Core.EF;

public class ConnectionOptions
{
    public string? ConnectionString { get; set; }
    public string DatabaseType { get; set; }
}

public static class DatabaseType
{
    public const string SqlServer = "SqlServer";
    public const string InMemory = "InMemory";
}