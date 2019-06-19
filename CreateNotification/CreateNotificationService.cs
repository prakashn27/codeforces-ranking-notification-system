using System;
using CreateNotification.DataAccess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ServiceStack;
using ServiceStack.Text;
using CreateNotification.Models;
using System.Linq;

namespace CreateNotification
{
    public class CreateNotificationService: IHostedService
    {

        private readonly IServiceScopeFactory scopeFactory;
        private Timer _timer;
        public CreateNotificationService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        // CreateNotificationDBContext _dbContext;
        // public CreateNotificationService(CreateNotificationDBContext context) {
        //     _dbContext = context;
        // }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Console.log("startAsync");
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CreateNotificationDBContext>();
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbitmquser", Password = "DEBmbwkSrzy9D1T9cJfa" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Rank",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        ChangedRank cr = JsonSerializer.DeserializeFromString<ChangedRank>(message);
                        // Console.WriteLine(" [x] Received {0} O:{1} N:{2}", cr.Handle, cr.OldRating, cr.NewRating);
                        var emails = (from user in dbContext.Followers where user.FollowerId == cr.Handle select user.EmailAddress).ToList();
                        using (var channel2 = connection.CreateModel())
                        {
                            channel2.QueueDeclare(queue: "Notification",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                            // var body = Encoding.UTF8.GetBytes(message);
                            foreach (var email in emails) 
                            {
                                Console.WriteLine(email);
                                Notification notification = new Notification(email, cr.Handle, cr.OldRating, cr.NewRating);
                                var body2 = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString<Notification>(notification));
                                channel2.BasicPublish(exchange: "Ranker",
                                                    routingKey: "Notification",
                                                    basicProperties: null,
                                                    body: body2);
                                Console.WriteLine(" [x] Sent {0}", notification.EmailAddress);
                            }
                        }
                        
                    };
                    channel.BasicConsume(queue: "Rank",
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
        private async Task fetchData(ChangedRank cr)
        {

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Console.log("startAsync");
            return Task.CompletedTask;

        }

    }
}