using System.Collections.Generic;

namespace BFF_GameMatch.Services.Dtos.Team
{
    public class CreateGroupRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? MemberIds { get; set; }
    }
}
