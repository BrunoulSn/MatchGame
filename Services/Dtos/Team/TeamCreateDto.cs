namespace BFF_GameMatch.Services.Dtos.Team
{
    public class TeamCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SportType { get; set; }
    }
}
