using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ServiceStack;
using ServiceStack.Text;
using CreateNotification.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreateNotification
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Console.WriteLine("Starting Send Notification service!");
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<CreateNotificationService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
            // while (true)
            // {
            //     var factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbitmquser", Password = "DEBmbwkSrzy9D1T9cJfa" };
            //     using (var connection = factory.CreateConnection())
            //     using (var channel = connection.CreateModel())
            //     {
            //         channel.QueueDeclare(queue: "Rank",
            //                              durable: true,
            //                              exclusive: false,
            //                              autoDelete: false,
            //                              arguments: null);

            //         var consumer = new EventingBasicConsumer(channel);
            //         consumer.Received += (model, ea) =>
            //         {
            //             var body = ea.Body;
            //             var message = Encoding.UTF8.GetString(body);
            //             ChangedRank cr = JsonSerializer.DeserializeFromString<ChangedRank>(message);
            //             if(cr.Handle == "prakashn") Console.WriteLine(" [x] Received {0} O:{1} N:{2}", cr.Handle, cr.OldRating, cr.NewRating);
            //         };
            //         channel.BasicConsume(queue: "Rank",
            //                              autoAck: true,
            //                              consumer: consumer);

            //         Console.WriteLine(" Press [enter] to exit.");
            //         Console.ReadLine();
            //     }
            // }
        }
    }
}
