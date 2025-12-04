namespace BFF_GameMatch.Services.Dtos
{
    public class BackendUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Skills { get; set; }
        public string? Availability { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}