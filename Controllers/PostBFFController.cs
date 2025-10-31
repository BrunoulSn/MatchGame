using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services;

namespace BFF_GameMatch.Controllers;

[ApiController]
[Route("bff/posts")]
public class PostBFFController : ControllerBase
{
    private readonly IHttpClientFactory _f;
    public PostBFFController(IHttpClientFactory f) => _f = f;

    [HttpGet("feed")]
    public async Task<IResult> Feed()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync("/posts/feed");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpGet("user/{userId:int}")]
    public async Task<IResult> PorUsuario(int userId)
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.GetAsync($"/posts/user/{userId}");
        return await ForwardingExtensions.ProxyResult(res);

    }

    [HttpPost]
    public async Task<IResult> Criar()
    {
        var cli = _f.CreateClient("backend");
        var res = await cli.SendAsync(Request.Forward(HttpMethod.Post, "/posts"));
        return await ForwardingExtensions.ProxyResult(res);

    }
}
