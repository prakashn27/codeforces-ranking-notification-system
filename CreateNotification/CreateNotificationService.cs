using CreateNotification.DataAccess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CreateNotification
{
    public class CreateNotificationService: IHostedService
    {
        // CreateNotificationDBContext _dbContext;
        // public CreateNotificationService(CreateNotificationDBContext context) {
        //     _dbContext = context;
        // }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Console.log("startAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Console.log("startAsync");
            return Task.CompletedTask;

        }

    }
}