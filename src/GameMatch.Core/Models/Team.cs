namespace GameMatch.Core.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SportType { get; set; }

        // 📍 Relação com o dono do time
        public int? OwnerId { get; set; }
        public User? Owner { get; set; }

        // 📍 Membros (N:N com User)
        public ICollection<User> Members { get; set; } = new List<User>();

        public string? Address { get; set; }
        public string? Photo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
