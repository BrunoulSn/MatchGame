using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services;

namespace BFF_GameMatch.Controllers;

[ApiController]
[Route("bff/notificacoes")]
public class NotificacaoBFFController : ControllerBase
{
    private readonly IHttpClientFactory _f;
    public NotificacaoBFFController(IHttpClientFactory f) => _f = f;

    [HttpGet("{userId:int}")]
    public async Task<IResult> Listar(int userId)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync($"/notifications/user/{userId}");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost("read/{id:int}")]
    public async Task<IResult> MarcarLida(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.PostAsync($"/notifications/read/{id}", content: null);
        return await ForwardingExtensions.ProxyResult(res);

    }
}
