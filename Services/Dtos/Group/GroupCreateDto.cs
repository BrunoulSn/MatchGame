namespace BFF_GameMatch.Services.Dtos.Group
{
    public class GroupCreateDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Sports { get; set; } // ⚠️ ADICIONE
        public int OwnerId { get; set; }
    }
}