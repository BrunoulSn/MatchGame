namespace BFF_GameMatch.Services.Dtos.Auth
{
    public class RegisterRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
