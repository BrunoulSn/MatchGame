namespace GameMatch.Api.DTOs
{
    public class PositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Characteristics { get; set; }
        public int SportId { get; set; }
    }

    public class CreatePositionDto
    {
        public string Name { get; set; } = default!;
        public string? Characteristics { get; set; }
        public int SportId { get; set; }
    }

    public class UpdatePositionDto
    {
        public string Name { get; set; } = default!;
        public string? Characteristics { get; set; }
    }
}
