using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FollowerManagementApi.Models;
using Polly;
using System;

namespace FollowerManagementApi.DataAccess
{  
    public class FollowerManagementDBContext : DbContext
    {
        public FollowerManagementDBContext(DbContextOptions<FollowerManagementDBContext> options) : base(options) {

        }

        public DbSet<Follower> Followers { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Follower>().HasKey(m => m.FollowerId);
            builder.Entity<Follower>().ToTable("FollowerTable");
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
