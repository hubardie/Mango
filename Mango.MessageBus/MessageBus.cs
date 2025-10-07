using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;


namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _serviceBusConnectionString = "Endpoint=sb://mangoweb-hubardie.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tSHLhp78HE8aDqTlrZ+yiirE6Xezv/NDZ+ASbCr2JrU=";
        //Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STRING") ?? throw new ArgumentNullException("SERVICE_BUS_CONNECTION_STRING environment variable is not set.");
        async Task IMessageBus.PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(_serviceBusConnectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage)) 
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
