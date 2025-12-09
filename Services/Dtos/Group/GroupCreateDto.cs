using System.Text.Json.Serialization;

namespace BFF_GameMatch.Services.Dtos.Group
{
    public class GroupCreateDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        [JsonPropertyName("sports")]
        public string? Sports { get; set; } // ⚠️ ADICIONE
        public int OwnerId { get; set; }
    }
}