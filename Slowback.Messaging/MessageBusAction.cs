using System;
using System.Threading.Tasks;

namespace Slowback.Messaging
{
    internal class MessageBusAction
    {
        public string Id { get; set; }
        public Func<object, Task> Action { get; set; }
    }
}