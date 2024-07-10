using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slowback.MessageBus
{
    public class MessageBus
    {
        private static MessageBus? _instance;
        private readonly Dictionary<string, object> _lastMessages = new Dictionary<string, object>();

        private readonly Dictionary<string, List<MessageBusAction>> _subscribers =
            new Dictionary<string, List<MessageBusAction>>();

        private MessageBus()
        {
        }

        public static MessageBus GetInstance()
        {
            if (_instance == null) _instance = new MessageBus();

            return _instance;
        }

        public Action Subscribe<T>(string message, Action<T> action)
        {
            var function = ConvertToFunction(action);

            if (!_subscribers.ContainsKey(message))
                _subscribers.Add(message, new List<MessageBusAction> { function });

            _subscribers[message].Add(function);

            return () => { _subscribers[message] = _subscribers[message].Where(f => f.Id != function.Id).ToList(); };
        }

        private MessageBusAction ConvertToFunction<T>(Action<T> action)
        {
            Func<object, Task> messageAction = o =>
            {
                action((o != null ? (T)o : default)!);
                return Task.CompletedTask;
            };

            return new MessageBusAction
            {
                Id = Guid.NewGuid().ToString(),
                Action = messageAction
            };
        }

        public void Publish<T>(string message, T payload)
        {
            AddToDictionary(message, payload);
            if (!_subscribers.TryGetValue(message, out var actions)) return;

            foreach (var action in actions) action.Action(payload);
        }

        private void AddToDictionary<T>(string message, T payload)
        {
            if (_lastMessages.ContainsKey(message))
                _lastMessages[message] = payload;
            else
                _lastMessages.Add(message, payload);
        }

        public async Task PublishAsync<T>(string message, T payload)
        {
            AddToDictionary(message, payload);
            if (!_subscribers.TryGetValue(message, out var actions)) return;

            foreach (var action in actions) await Task.Run(() => action.Action(payload));
        }

        public T GetLastMessage<T>(string message)
        {
            return _lastMessages.ContainsKey(message) ? (T)_lastMessages[message] : default;
        }

        public void ClearSubscribers()
        {
            _subscribers.Clear();
        }

        public void ClearMessages()
        {
            _lastMessages.Clear();
        }
    }
}