using GameMatch.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace GameMatch.Infrastructure;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();




    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Group>().HasIndex(x => x.Name).IsUnique();
        b.Entity<GroupMember>().HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();
    }
}
