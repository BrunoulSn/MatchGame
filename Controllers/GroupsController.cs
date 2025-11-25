using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBffProject.Services;
using MyBffProject.Services.Dtos.Backend;

namespace MyBffProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class GroupsController : ControllerBase
    {
        private readonly IBackendProxyService _backend;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IBackendProxyService backend, ILogger<GroupsController> logger)
        {
            _backend = backend;
            _logger = logger;
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
            var g = await _backend.GetGroupAsync(id, cancellationToken);
            return g is null ? NotFound() : Ok(g);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupCreateDto dto, CancellationToken cancellationToken = default)
        {
            var created = await _backend.CreateGroupAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GroupUpdateDto dto, CancellationToken cancellationToken = default)
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
        public async Task<IActionResult> Join(int id, [FromBody] JoinDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await _backend.JoinGroupAsync(id, dto, cancellationToken);
            return Ok(resp);
        }

        [HttpPost("{id:int}/remMembro")]
        public async Task<IActionResult> Kick(int id, [FromBody] KickDto dto, CancellationToken cancellationToken = default)
        {
            await _backend.KickMemberAsync(id, dto, cancellationToken);
            return Ok();
        }

        [HttpPost("{id:int}/reagrupar")]
        public async Task<IActionResult> Reorder(int id, [FromBody] ReorderDto dto, CancellationToken cancellationToken = default)
        {
            await _backend.ReorderGroupAsync(id, dto, cancellationToken);
            return Ok();
        }
    }
}
