using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slowback.Messaging
{
    public static class MessageBus
    {
        private static readonly Dictionary<string, object> _lastMessages = new Dictionary<string, object>();

        private static readonly Dictionary<string, List<MessageBusAction>> _subscribers =
            new Dictionary<string, List<MessageBusAction>>();

        private static List<MessageBusAction> _allMessageSubscribers = new List<MessageBusAction>();

        public static Action Subscribe<T>(string message, Action<T> action)
        {
            var function = ConvertToFunction(action);

            if (!_subscribers.ContainsKey(message))
                _subscribers.Add(message, new List<MessageBusAction> { function });
            else
                _subscribers[message].Add(function);

            return () => { _subscribers[message] = _subscribers[message].Where(f => f.Id != function.Id).ToList(); };
        }

        public static Action SubscribeToAllMessages<T>(Action<T> action)
        {
            var function = ConvertToFunction(action);
            _allMessageSubscribers.Add(function);

            return () => { _allMessageSubscribers = _allMessageSubscribers.Where(f => f.Id != function.Id).ToList(); };
        }

        private static MessageBusAction ConvertToFunction<T>(Action<T> action)
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

        public static void Publish<T>(string message, T payload)
        {
            AddToDictionary(message, payload);

            foreach (var action in _allMessageSubscribers) TryPublishMessage(action.Action, payload);

            if (!_subscribers.TryGetValue(message, out var actions)) return;

            foreach (var action in actions) action.Action(payload);
        }

        private static void TryPublishMessage<T>(Func<object, Task> action, T payload)
        {
            try
            {
                action(payload);
            }
            catch
            {
                action(null);
            }
        }

        private static void AddToDictionary<T>(string message, T payload)
        {
            if (_lastMessages.ContainsKey(message))
                _lastMessages[message] = payload;
            else
                _lastMessages.Add(message, payload);
        }

        public static async Task PublishAsync<T>(string message, T payload)
        {
            AddToDictionary(message, payload);
            foreach (var action in _allMessageSubscribers) TryPublishMessage(action.Action, payload);

            if (!_subscribers.TryGetValue(message, out var actions)) return;

            foreach (var action in actions) await Task.Run(() => action.Action(payload));
        }

        public static T GetLastMessage<T>(string message)
        {
            return _lastMessages.ContainsKey(message) ? (T)_lastMessages[message] : default;
        }

        public static void ClearSubscribers()
        {
            _subscribers.Clear();
            _allMessageSubscribers.Clear();
        }

        public static void ClearMessages()
        {
            _lastMessages.Clear();
        }
    }
}