﻿using System;

namespace GameMatch.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Caption { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
