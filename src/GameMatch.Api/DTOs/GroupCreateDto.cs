public class GroupCreateDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int SportId { get; set; }
    public int MaxMembers { get; set; }
    public int OwnerId { get; set; }
}
public class GroupUpdateDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int MaxMembers { get; set; }
}
public class GroupResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int SportId { get; set; }
    public int MaxMembers { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}
