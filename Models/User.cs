using System.Collections.Generic;

namespace MyBffProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CPF { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        // Password mantido no model, trate com cuidado (não expor em DTOs)
        public string? Password { get; set; }

        // Times que o usuário participa (many-to-many)
        public List<Team> Teams { get; set; } = new();
    }
}