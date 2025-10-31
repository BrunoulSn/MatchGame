using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services;

namespace BFF_GameMatch.Controllers;

[ApiController]
[Route("bff/grupo")]
public class GrupoBFFController : ControllerBase
{
    private readonly IHttpClientFactory _f;
    public GrupoBFFController(IHttpClientFactory f) => _f = f;

    [HttpGet]
    public async Task<IResult> Listar()
    {
        var cli = _f.CreateClient("backend");
        var qs = Request.QueryString.HasValue ? Request.QueryString.Value : "";
        var res = await cli.GetAsync("/groups" + qs);
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpGet("{id:int}")]
    public async Task<IResult> Obter(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync($"/groups/{id}");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost("cadastrar")]
    public async Task<IResult> Cadastrar()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, "/groups"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPut("{id:int}")]
    public async Task<IResult> Atualizar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Put, $"/groups/{id}"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> Deletar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.DeleteAsync($"/groups/{id}");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost("{id:int}/join")]
    public async Task<IResult> Entrar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, $"/groups/{id}/join"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost("{id:int}/kick")]
    public async Task<IResult> RemoverMembro(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, $"/groups/{id}/kick"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost("{id:int}/reorder")]
    public async Task<IResult> Reordenar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, $"/groups/{id}/reorder"));
        return await ForwardingExtensions.ProxyResult(res);

    }
}
