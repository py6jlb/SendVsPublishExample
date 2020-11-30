using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Producer
{
    class Program
    {
        private static async Task Main(string[] args)
        {
           
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("rabbit");
                    h.Password("rabbit");
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
           // var sendEndpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://rabbit/event-listener-5"));


            await busControl.StartAsync(source.Token);
            try
            {
                var i = 0;
                //await sendEndpoint.Send(new Message { HelloMsg = $"start" });
                while (i<1000)
                {
                    //await sendEndpoint.Send(new Message { HelloMsg = $"{i}"});
                    await busControl.Publish(new Message { HelloMsg = $"{i}" });
                    i++;
                }
                //await sendEndpoint.Send(new Message { HelloMsg = $"end" });

                do
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;
                    //await sendEndpoint.Send(new Message { HelloMsg = value });
                    await busControl.Publish(new Message { HelloMsg = value });
                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }

            //var sendEndpoint = await bus.GetSendEndpoint(new Uri("rabbitmq://rabbit/q"));
            //await sendEndpoint.Send(new Message {HelloMsg = "Sended Hello World"});

        }


    }
}
