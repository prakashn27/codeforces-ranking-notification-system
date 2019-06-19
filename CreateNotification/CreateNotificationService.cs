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

namespace CreateNotification
{
    public class CreateNotificationService: IHostedService
    {

        private readonly IServiceScopeFactory scopeFactory;

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
        
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Console.log("startAsync");
            return Task.CompletedTask;

        }

    }
}