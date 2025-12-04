namespace BFF_GameMatch.Services.Dtos.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Skills { get; set; }
        public string? Availability { get; set; }
    }
}
