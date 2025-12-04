using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BFF_GameMatch.Authentication
{
    // Handler simples para desenvolvimento: se enviar header X-User-Id ou Authorization: Bearer {id}, cria uma identidade com claim NameIdentifier.
    public class DevAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DevAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? userId = null;

            if (Request.Headers.TryGetValue("X-User-Id", out var headerVal))
            {
                userId = headerVal.ToString();
            }
            else if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var val = authHeader.ToString();
                if (val.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    userId = val.Substring("Bearer ".Length).Trim();
                }
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId), new Claim(ClaimTypes.Name, userId) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = "application/json; charset=utf-8";
            var payload = new { mensagem = "Não foi possível identificar o usuário autenticado." };
            return Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
