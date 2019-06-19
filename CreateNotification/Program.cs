using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ServiceStack;
using ServiceStack.Text;
using CreateNotification.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CreateNotification.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CreateNotification
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Console.WriteLine("Starting Send Notification service!");
            var host = CreateHostBuilder(args).Build();
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
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddEnvironmentVariables("DOTNET_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddTransient<IMessageHandler>((svc) =>
                    // {
                    //     var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                    //     string rabbitMQHost = rabbitMQConfigSection["Host"];
                    //     string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                    //     string rabbitMQPassword = rabbitMQConfigSection["Password"];
                    //     // return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "Rankger", "Rank", ""); ;
                    // });

                    // services.AddTransient<AuditlogManagerConfig>((svc) =>
                    // {
                    //     var auditlogConfigSection = hostContext.Configuration.GetSection("Auditlog");
                    //     string logPath = auditlogConfigSection["path"];
                    //     return new AuditlogManagerConfig { LogPath = logPath };
                    // });
                    var sqlConnectionString = "server=localhost,1434;user id=sa;password=8jkGh47hnDw89Haq8LN2;database=UserManagement;";
                    services.AddDbContext<CreateNotificationDBContext>(options => options.UseSqlServer(sqlConnectionString));

                    services.AddHostedService<CreateNotificationService>();
                })
                // .UseSerilog((hostContext, loggerConfiguration) =>
                // {
                //     loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                // })
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }
}
