namespace BFF_GameMatch.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; } // <-- PROPRIEDADE Phone ADICIONADA
        public DateOnly? BirthDate { get; set; }
        public string? Skills { get; set; }
        public string? Availability { get; set; }
    }
}