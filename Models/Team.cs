using BFF_GameMatch.Models;
using System;
using System.Collections.Generic;

namespace MyBffProject.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SportType { get; set; }

        // Owner como relação opcional
        public int? OwnerId { get; set; }
        public User? Owner { get; set; }

        // Membros (many-to-many)
        public List<User> Members { get; set; } = new();

        public string? Address { get; set; }
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}