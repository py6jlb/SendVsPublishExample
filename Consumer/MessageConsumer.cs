using System;
using System.Threading.Tasks;
using MassTransit;
using Producer;

namespace Consumer
{
    public class MessageConsumer : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine($"Received: {context.Message.HelloMsg}");
            await Task.CompletedTask;

        }
    }
}