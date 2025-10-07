using System.Runtime.InteropServices.ObjectiveC;

namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_name); // names must be unique between topic and queues

    }
}
