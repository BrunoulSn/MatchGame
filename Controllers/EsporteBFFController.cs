using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services;

namespace BFF_GameMatch.Controllers;

[ApiController]
[Route("bff/esportes")]
public class EsporteBFFController : ControllerBase
{
    private readonly IHttpClientFactory _f;
    public EsporteBFFController(IHttpClientFactory f) => _f = f;

    [HttpGet]
    public async Task<IResult> Listar()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync("/sports");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpGet("{id:int}")]
    public async Task<IResult> Obter(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync($"/sports/{id}");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost]
    public async Task<IResult> Criar()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, "/sports"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPut("{id:int}")]
    public async Task<IResult> Atualizar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Put, $"/sports/{id}"));
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> Deletar(int id)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.DeleteAsync($"/sports/{id}");
        return await ForwardingExtensions.ProxyResult(res);

    }
}
