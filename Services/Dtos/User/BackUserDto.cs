using System;
using System.Collections.Generic;

namespace MyBffProject.Services.Dtos.Backend
{
    // Users (matches backend User model used in controllers)
    public class BackUserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Skills { get; set; }
        public string? Availability { get; set; }
    }

    // Groups
    public class BackGroupDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BackGroupCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int OwnerId { get; set; }
    }

    public class BackGroupUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class BackGroupResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BackJoinDto
    {
        public int UserId { get; set; }
    }

    public class BackKickDto
    {
        public int ActorId { get; set; }
        public int MemberUserId { get; set; }
    }

    public class BackReorderItem
    {
        public int UserId { get; set; }
    }

    public class BackReorderDto
    {
        public List<BackReorderItem> Items { get; set; } = new();
    }
}