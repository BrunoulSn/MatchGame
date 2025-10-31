using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services;

namespace BFF_GameMatch.Controllers;

[ApiController]
[Route("bff/usuario")]
public class UsuarioBFFController : ControllerBase
{
    private readonly IHttpClientFactory _f;
    public UsuarioBFFController(IHttpClientFactory f) => _f = f;

    [HttpPost("login")]
    public async Task<IResult> Login()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, "/auth/login"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost("cadastrar")]
    public async Task<IResult> Cadastrar()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, "/auth/register"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpGet]
    public async Task<IResult> Listar()
    {
        var cli = _f.CreateClient("backend");
        var qs = Request.QueryString.HasValue ? Request.QueryString.Value : "";
        var res = await cli.GetAsync("/users" + qs);
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpGet("{id:int}")]
    public async Task<IResult> Obter(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync($"/users/{id}");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPut("{id:int}")]
    public async Task<IResult> Atualizar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Put, $"/users/{id}"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> Deletar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.DeleteAsync($"/users/{id}");
        return await ForwardingExtensions.ProxyResult(res);

    }
}
