using System;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserManagementApi.Commands
{
    public class RegisterUser : Command
    {
        public readonly string UserId;
        public readonly string Name;
        public readonly string EmailAddress;
        public readonly string TelephoneNumber;
        public RegisterUser(Guid messageId, string userId, string name, string emailAddress, string telephoneNumber) : base(messageId)
        {
            Name = name;
            TelephoneNumber = telephoneNumber;
            EmailAddress = emailAddress;
            UserId = userId;
        }
    }
}