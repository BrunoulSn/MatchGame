namespace GameMatch.Api.DTOs;

public class SportDto
{
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Description { get; set; }
}

public class SportUpdateDto
{
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Description { get; set; }
}
public class SportResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Description { get; set; }
}
