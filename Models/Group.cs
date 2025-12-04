namespace BFF_GameMatch.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relação com os membros do grupo
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    }
}
