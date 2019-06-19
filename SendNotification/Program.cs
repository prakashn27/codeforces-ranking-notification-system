using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendNotification.NotificationChannels;
using Serilog;

namespace SendNotification
{
    class Program
    {
        // static void Main(string[] args)
        // {
        // Console.WriteLine("Starting Send Notification service!");

        // var factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbitmquser", Password = "DEBmbwkSrzy9D1T9cJfa" };
        // using (var connection = factory.CreateConnection())
        // using (var channel = connection.CreateModel())
        // {
        //     channel.QueueDeclare(queue: "Notification",
        //                             durable: true,
        //                             exclusive: false,
        //                             autoDelete: false,
        //                             arguments: null);

        //     var consumer = new EventingBasicConsumer(channel);
        //     consumer.Received += (model, ea) =>
        //     {
        //         var body = ea.Body;
        //         var message = Encoding.UTF8.GetString(body);
        //         Console.WriteLine(" [x] Received {0}", message);
        //     };
        //     channel.BasicConsume(queue: "Notification",
        //                             autoAck: true,
        //                             consumer: consumer);

        //     Console.WriteLine(" Press [enter] to exit.");
        //     Console.ReadLine();
        // }

        // }
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
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
                    //     return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "Pitstop", "Notifications", ""); ;
                    // });

                    // services.AddTransient<INotificationRepository>((svc) =>
                    // {
                    //     var sqlConnectionString = hostContext.Configuration.GetConnectionString("NotificationServiceCN");
                    //     return new SqlServerNotificationRepository(sqlConnectionString);
                    // });

                    services.AddTransient<IEmailNotifier>((svc) =>
                    {
                        var mailConfigSection = hostContext.Configuration.GetSection("Email");
                        string mailHost = mailConfigSection["Host"];
                        int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
                        string mailUserName = mailConfigSection["User"];
                        string mailPassword = mailConfigSection["Pwd"];
                        return new SMTPEmailNotifier(mailHost, mailPort, mailUserName, mailPassword);
                    });

                    services.AddHostedService<NotificationManager>();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }
}
