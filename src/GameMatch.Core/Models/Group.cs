namespace GameMatch.Core.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

public class GroupMember
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public MemberRole Role { get; set; } = MemberRole.Member;
}