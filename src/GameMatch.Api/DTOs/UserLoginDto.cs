namespace GameMatch.Api.DTOs
{
    public class UserLoginDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!; // 🔹 Mantém senha visível apenas para login interno
    }
}
