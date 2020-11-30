using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Consumer
{
    class Program
    {
        private static async Task Main(string[] args)
        {

            var queueName = Environment.GetEnvironmentVariable("DOTNET_Q");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbit", "/", h =>
                {
                    h.Username("rabbit");
                    h.Password("rabbit");
                });

                cfg.ReceiveEndpoint(queueName, e =>
                {
                    e.Consumer<MessageConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
