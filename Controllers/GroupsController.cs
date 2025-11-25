using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services.Dtos.Team;
using MyBffProject.Services.Dtos.Backend;
using MyBffProject.Services;
using System.Net.Http;

namespace BFF_GameMatch.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class GroupsController : ControllerBase
    {
        private readonly IBackendProxyService _backend;
        private readonly ILogger<GroupsController> _logger;
        private readonly HttpClient _httpClient;


        public GroupsController(IBackendProxyService backend, ILogger<GroupsController> logger, HttpClient httpClient)
        {
            _backend = backend;
            _logger = logger;
            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> List(CancellationToken cancellationToken = default)
        {
            var groups = await _backend.GetGroupsAsync(cancellationToken);
            return Ok(groups);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var group = await _backend.GetGroupAsync(id, cancellationToken);
            return group is null ? NotFound() : Ok(group);
        }

        [HttpPost]
        public async Task<BackGroupDto> CreateGroupAsync(CreateGroupRequest dto, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("api/groups", dto, cancellationToken);
            response.EnsureSuccessStatusCode();

            var group = await response.Content.ReadFromJsonAsync<BackGroupDto>(cancellationToken: cancellationToken);
            return group ?? throw new InvalidOperationException("Erro ao criar grupo");
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeamUpdateDto dto, CancellationToken cancellationToken = default)
        {
            await _backend.UpdateGroupAsync(id, dto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _backend.DeleteGroupAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id:int}/addMembro")]
        public async Task<IActionResult> Join(int id, [FromBody] BackJoinDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _backend.JoinGroupAsync(id, dto, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{id:int}/remMembro")]
        public async Task<IActionResult> Kick(int id, [FromBody] BackKickDto dto, CancellationToken cancellationToken = default)
        {
            await _backend.KickMemberAsync(id, dto, cancellationToken);
            return Ok();
        }

        [HttpPost("{id:int}/reagrupar")]
        public async Task<IActionResult> Reorder(int id, [FromBody] BackReorderItem dto, CancellationToken cancellationToken = default)
        {
            await _backend.ReorderGroupAsync(id, dto, cancellationToken);
            return Ok();
        }
    }
}
