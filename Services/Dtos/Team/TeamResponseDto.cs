namespace BFF_GameMatch.Services.Dtos.Team
{
    public class TeamResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string SportType { get; set; } = default!;
        public string? Address { get; set; }
        public string? Photo { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
