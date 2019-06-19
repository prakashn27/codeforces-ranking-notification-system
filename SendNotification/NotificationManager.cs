using Microsoft.Extensions.Hosting;
// using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack;
using ServiceStack.Text;
using SendNotification.NotificationChannels;
using SendNotification.models;

namespace SendNotification
{
    public class NotificationManager: IHostedService
    {
        private IEmailNotifier _emailNotifier;
        private readonly IServiceScopeFactory scopeFactory;

        public NotificationManager(IServiceScopeFactory scopeFactory, IEmailNotifier emailNotifier) {
            _emailNotifier = emailNotifier;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Console.log("startAsync");
            using (var scope = scopeFactory.CreateScope())
            {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbitmquser", Password = "DEBmbwkSrzy9D1T9cJfa" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Notification",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var eabody = ea.Body;
                        var message = Encoding.UTF8.GetString(eabody);
                        Console.WriteLine(message);
                        Notification notification = JsonSerializer.DeserializeFromString<Notification>(message);
                        Console.WriteLine("Sent notification to: {0}", notification.EmailAddress);

                        // // build notification body
                        StringBuilder body = new StringBuilder();
                        body.AppendLine($"Dear User,\n");
                        body.AppendLine($"There has beed a change in Rank of {notification.FollowerId}:\n");
                        body.AppendLine($"Old Rank: {notification.OldRank} \nNew Rank: {notification.NewRank} ");

                        // send notification
                        _emailNotifier.SendEmailAsync(
                            notification.EmailAddress, "noreply@pitstop.nl", "Rank Changed for "+ notification.FollowerId, body.ToString());

                    };
                    channel.BasicConsume(queue: "Notification",
                                            autoAck: true,
                                            consumer: consumer);

                    // Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            // _timer = new Timer(DoWork, null, TimeSpan.Zero,
            // TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // _messageHandler.Stop();
            return Task.CompletedTask;
        }
    }
}