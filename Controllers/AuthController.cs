using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BFF_GameMatch.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _http;

        public AuthController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("GameMatchApi");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", dto);

            var body = await response.Content.ReadAsStringAsync();
            return Content(body, "application/json");
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
