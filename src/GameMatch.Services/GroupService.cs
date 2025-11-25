using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Services;

public class GroupService
{
    private readonly AppDb _db;
    public GroupService(AppDb db) { _db = db; }

    public async Task<Group> Create(string name, string? desc, int sportId, int maxMembers, int ownerId)
    {

        if (await _db.Groups.AnyAsync(g => g.Name == name))
            throw new InvalidOperationException("Nome de grupo já existe");

        var g = new Group { Name = name, Description = desc, OwnerId = ownerId};
        _db.Groups.Add(g);
        await _db.SaveChangesAsync();

        _db.GroupMembers.Add(new GroupMember { GroupId = g.Id, UserId = ownerId, Role = MemberRole.Owner });
        await _db.SaveChangesAsync();
        return g;
    }
    public async Task Join(int groupId, int userId)
    {
        if (await _db.GroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId))
            throw new InvalidOperationException("Já é membro");
        var g = await _db.Groups.FindAsync(groupId) ?? throw new KeyNotFoundException();
        var count = await _db.GroupMembers.CountAsync(m => m.GroupId == groupId);
        await _db.SaveChangesAsync();
    }
}
