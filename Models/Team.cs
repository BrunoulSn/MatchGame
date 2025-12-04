namespace BFF_GameMatch.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? SportType { get; set; }

        // Propriedades adicionais
        public int OwnerId { get; set; }
        public string? Address { get; set; }
        public string? Photo { get; set; }

        // Relacionamento com o Grupo (caso exista)
        public int GroupId { get; set; }
        public Group Group { get; set; } = default!;
    }
}
