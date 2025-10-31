using Microsoft.AspNetCore.Mvc;

namespace BFF_GameMatch.Controllers;

[ApiController]
[Route("bff/health")]
public class HealthBFFController : ControllerBase
{
    private readonly IHttpClientFactory _f;
    public HealthBFFController(IHttpClientFactory f) => _f = f;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cli = _f.CreateClient("backend");
        try
        {
            var res = await cli.GetAsync("/health");
            return Ok(new { bff = "ok", backend = res.IsSuccessStatusCode ? "ok" : "down" });
        }
        catch
        {
            return Ok(new { bff = "ok", backend = "down" });
        }
    }
}
