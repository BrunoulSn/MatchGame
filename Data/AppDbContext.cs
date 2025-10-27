using System.Collections.Generic;
using BFF_GameMatch.Models;
using Microsoft.EntityFrameworkCore;
using MyBffProject.Models;

namespace MyBffProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Name).IsRequired().HasMaxLength(150);
                b.Property(t => t.Description).HasMaxLength(2000);
                b.Property(t => t.SportType).HasMaxLength(100);
                b.Property(t => t.Photo).HasMaxLength(500);
                b.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETUTCDATE()");

                // Owner: FK shadow property OwnerId (pode ser null)
                b.HasOne<User>()
                 .WithMany()
                 .HasForeignKey("OwnerId")
                 .OnDelete(DeleteBehavior.SetNull);

                // Many-to-many Team <-> User (Members <-> Teams) via join table TeamMembers
                b.HasMany(t => t.Members)
                 .WithMany(u => u.Teams)
                 .UsingEntity<Dictionary<string, object>>(
                    "TeamMembers",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Team>().WithMany().HasForeignKey("TeamId").OnDelete(DeleteBehavior.Cascade)
                 );
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(u => u.Id).IsRequired();
                b.Property(u => u.Name).HasMaxLength(150);
                b.Property(u => u.Email).HasMaxLength(200);
                b.Property(u => u.CPF).HasMaxLength(20);
                b.Property(u => u.Phone).HasMaxLength(50);
                // Não mapear Password para APIs (permanece no model, mas trate com cuidado)
            });
        }
    }
}
