using BFF_GameMatch.Services.Dtos.Team;

namespace BFF_GameMatch.Services.Dtos.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Skills { get; set; }
        public string? Availability { get; set; }

        // Mantém o relacionamento com times, caso seja necessário no futuro
        public List<TeamDto>? Teams { get; set; }
    }
}
