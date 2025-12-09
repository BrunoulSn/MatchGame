using System.Text.Json.Serialization;

namespace BFF_GameMatch.Services.Dtos.Group
{
    public class GroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("sports")] // 👈 adiciona para o GET mostrar o esporte
        public string? Sports { get; set; }
    }
}
