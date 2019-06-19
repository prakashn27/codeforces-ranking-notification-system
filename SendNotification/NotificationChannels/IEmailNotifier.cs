using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SendNotification.NotificationChannels
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(string to, string from, string subject, string body);
    }
}