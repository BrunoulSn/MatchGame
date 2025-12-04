using BFF_GameMatch.Services.Dtos.Team;

namespace BFF_GameMatch.Services.Dtos.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } // 🔥 ADICIONE se quiser atualizar senha
        public List<TeamDto>? Teams { get; set; }
    }
}