namespace GameMatch.Api.DTOs
{
    public class GroupResponseDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int MaxMembers { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
