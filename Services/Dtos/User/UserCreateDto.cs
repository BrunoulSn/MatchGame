namespace BFF_GameMatch.Services.Dtos.User
{
    public class UserCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Senha em texto
        public DateOnly? BirthDate { get; set; }
        public string Skills { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;
    }
}