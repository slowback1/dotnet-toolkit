using Slowback.Logger.LoggingEngines;

namespace Slowback.Logger;

public class Logger
{
    private static Logger? _instance;

    public static readonly string DefaultLoggingMessage = "LOGGERLOG";
    private readonly string _loggingMessage;

    private Logger(List<ILoggingEngine> loggingEngineses, string? defaultLoggingMessage = null)
    {
        LoggingEngines = loggingEngineses;
        _loggingMessage = defaultLoggingMessage ?? DefaultLoggingMessage;

        ListenForILoggableMessages();
        ListenForStringMessages();
    }

    private List<ILoggingEngine> LoggingEngines { get; }
    private Action UnsubscribeFromAllMessages { get; set; }
    private Action UnsubscribeFromDefaultLoggingMessage { get; set; }

    public static void EnableLogging(List<ILoggingEngine> loggingEngines, string? defaultLoggingMessage = null)
    {
        if (_instance is not null) return;

        _instance = new Logger(loggingEngines, defaultLoggingMessage);
    }

    internal static void DisableLogging()
    {
        if (_instance is null) return;

        _instance.UnsubscribeFromAllMessages();
        _instance.UnsubscribeFromDefaultLoggingMessage();

        _instance = null;
    }

    private void ListenForILoggableMessages()
    {
        UnsubscribeFromAllMessages = MessageBus.MessageBus.GetInstance().SubscribeToAllMessages<ILoggable?>(message =>
        {
            if (message is null) return;

            LogMessage(message.ToLoggableString());
        });
    }

    private void ListenForStringMessages()
    {
        UnsubscribeFromDefaultLoggingMessage = MessageBus.MessageBus.GetInstance().Subscribe<string?>(
            _loggingMessage, message =>
            {
                if (message is null) return;

                LogMessage(message);
            });
    }

    private void LogMessage(string message)
    {
        foreach (var loggingEngine in LoggingEngines) loggingEngine.Log(message);
    }
}