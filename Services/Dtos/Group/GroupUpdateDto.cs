using System.Text.Json.Serialization;

namespace BFF_GameMatch.Services.Dtos.Group
{
    public class GroupUpdateDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        [JsonPropertyName("sports")] // 👈 garante compatibilidade com o backend
        public string? Sports { get; set; }
    }
}
