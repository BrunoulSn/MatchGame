using System.ComponentModel.DataAnnotations;

public class TeamUpdateDto
{
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SportType { get; set; }
    public string? Address { get; set; }
    public string? Photo { get; set; }
}