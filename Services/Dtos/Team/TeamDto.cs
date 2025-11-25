using BFF_GameMatch.Services.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace BFF_GameMatch.Services.Dtos.Team
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SportType { get; set; }
        public string? Address { get; set; }
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto? Owner { get; set; }
        public int MemberCount { get; set; }
    }
}