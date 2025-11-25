using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBffProject.Services;
using BFF_GameMatch.Services.Dtos.Auth;

namespace MyBffProject.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IBackendProxyService _proxy;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IBackendProxyService proxy, ILogger<AuthController> logger)
        {
            _proxy = proxy;
            _logger = logger;
        }

      /*  [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var resp = await _proxy.RegisterAsync(dto, ct);
            var result = new ContentResult();
            result.Content = resp.Content ?? string.Empty;
            result.ContentType = resp.ContentType ?? "application/json";
            result.StatusCode = resp.StatusCode;
            return result;
        }*/

        /*[HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var resp = await _proxy.LoginAsync(dto, ct);
            var result = new ContentResult();
            result.Content = resp.Content ?? string.Empty;
            result.ContentType = resp.ContentType ?? "application/json";
            result.StatusCode = resp.StatusCode;
            return result;
        }*/
    }
}
