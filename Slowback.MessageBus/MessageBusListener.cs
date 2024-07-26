using System.Threading.Tasks;

namespace Slowback.MessageBus
{
    public abstract class MessageBusListener<T>
    {
        private readonly string _topic;

        protected MessageBusListener(string topic)
        {
            _topic = topic;
            ListenForMessages();
        }

        private void ListenForMessages()
        {
            MessageBus.GetInstance().Subscribe<T>(_topic, message => OnMessage(message));
        }

        public abstract Task OnMessage(T message);
    }
}