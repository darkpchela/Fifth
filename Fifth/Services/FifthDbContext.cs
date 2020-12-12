using Fifth.Models;
using Microsoft.EntityFrameworkCore;

namespace Fifth.Services
{
    public class FifthDbContext : DbContext
    {
        public DbSet<GameSession> GameSessions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<SessionTag> SessionTags { get; set; }

        public FifthDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionTag>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}