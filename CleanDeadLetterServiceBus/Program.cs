using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace CleanDeadLetterServiceBus
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "";
            string queueName = "";
            int maxMessages = int.MaxValue;

            var client = new ServiceBusClient(connectionString);

            var options = new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock, SubQueue = SubQueue.DeadLetter };
            var receiver = client.CreateReceiver(queueName, options);

            Task.Run(async () =>
            {
                var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages);

                if (receivedMessages != null)
                    foreach (var message in receivedMessages)
                        await receiver.CompleteMessageAsync(message);
            }).Wait();
        }
    }
}
