using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagementApi.Models;
using Polly;
using System;

namespace UserManagementApi
{  
    public class UserManagementDBContext : DbContext
    {
        public UserManagementDBContext(DbContextOptions<UserManagementDBContext> options) : base(options) {

        }

        public DbSet<User> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>().HasKey(m => m.UserId);
            builder.Entity<User>().ToTable("User");
            base.OnModelCreating(builder);
        }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
