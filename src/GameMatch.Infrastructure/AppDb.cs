using GameMatch.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Infrastructure
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
        public DbSet<Team> Teams => Set<Team>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Índices existentes
            b.Entity<User>().HasIndex(x => x.Email).IsUnique();
            b.Entity<Group>().HasIndex(x => x.Name).IsUnique();
            b.Entity<GroupMember>().HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();

            // 🔗 Team.Owner (1:N)
            b.Entity<Team>()
                .HasOne(t => t.Owner)
                .WithMany()
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔗 Team.Members (N:N)
            b.Entity<Team>()
                .HasMany(t => t.Members)
                .WithMany()
                .UsingEntity(j => j.ToTable("TeamMembers"));
        }
    }
}
