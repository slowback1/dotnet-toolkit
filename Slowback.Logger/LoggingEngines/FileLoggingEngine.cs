namespace Slowback.Logger.LoggingEngines;

public class FileLoggingEngineSettings
{
    public FileLoggingTimestampFormat TimestampFormat { get; set; }
}

public enum FileLoggingTimestampFormat
{
    None,
    Short,
    Long
}

//TODO: Implement a configurable log rotation strategy
public class FileLoggingEngine : ILoggingEngine
{
    private static readonly FileLoggingEngineSettings DefaultSettings = new()
    {
        TimestampFormat = FileLoggingTimestampFormat.None
    };

    public FileLoggingEngine()
    {
        Settings = DefaultSettings;
    }

    public FileLoggingEngine(FileLoggingEngineSettings settings)
    {
        Settings = settings;
    }

    private FileLoggingEngineSettings Settings { get; }

    public void Log(string message)
    {
        if (!FileExists())
            CreateFile();
        AppendToFile(message);
    }

    private bool FileExists()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        return File.Exists(filePath);
    }

    private void CreateFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        File.WriteAllText(filePath, string.Empty);
    }

    private void AppendToFile(string message)
    {
        var filePath = Path.Combine(Path.GetTempPath(), "test.log");
        File.AppendAllText(filePath, GetLogMessage(message) + Environment.NewLine);
    }

    private string GetLogMessage(string baseMessage)
    {
        return $"{GetTimestamp()}{baseMessage}";
    }

    private string GetTimestamp()
    {
        return Settings.TimestampFormat switch
        {
            FileLoggingTimestampFormat.None => string.Empty,
            FileLoggingTimestampFormat.Short => $"({DateTime.Now:HH:mm:ss}) ",
            FileLoggingTimestampFormat.Long => $"({DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}) ",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}