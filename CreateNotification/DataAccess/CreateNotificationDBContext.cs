using Microsoft.EntityFrameworkCore;
using CreateNotification.Models;

namespace CreateNotification.DataAccess
{
    public class CreateNotificationDBContext : DbContext
    {

        public CreateNotificationDBContext(DbContextOptions<CreateNotificationDBContext> options) : base(options)
        {

        }

        public DbSet<Follower> Followers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Follower>().HasKey(m => m.FollowerId);
            builder.Entity<Follower>().ToTable("FollowerTable");
            base.OnModelCreating(builder);
        }

    }
}