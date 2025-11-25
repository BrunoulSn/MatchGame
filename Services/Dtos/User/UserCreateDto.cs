using BFF_GameMatch.Models;
using BFF_GameMatch.Services.Dtos.Team;

namespace BFF_GameMatch.Services.Dtos.User
{
    public class UserCreateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
}
