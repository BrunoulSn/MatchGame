namespace BFF_GameMatch.Services.Dtos.Team
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SportType { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Photo { get; set; }
        public int OwnerId { get; set; }
    }
}
