using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Fifth.Models;

#nullable disable

namespace Fifth.Services.DataContext
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SessionData> Sessions { get; set; }
        public virtual DbSet<SessionTag> SessionTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=Default");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<SessionData>(entity =>
            {
                entity.HasIndex(e => e.CreatorId, "IX_GameInfoDatas_CreatorId");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_GameSessions_To_Users");
            });

            modelBuilder.Entity<SessionTag>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.SessionId, "IX_SessionTags_SessionId");

                entity.HasOne(d => d.Session)
                    .WithMany()
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SessionTags_To_GameSessions");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Text, "UQ_Text")
                    .IsUnique();

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Login, "UQ_Login")
                    .IsUnique();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
