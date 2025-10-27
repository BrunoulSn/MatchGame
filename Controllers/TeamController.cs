using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BFF_GameMatch.Models;
using BFF_GameMatch.Services.Dtos.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBffProject.Models;
using MyBffProject.Services;
using MyBffProject.Services.Results;

namespace MyBffProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;
        private readonly ILogger<TeamController> _logger;

        public TeamController(ITeamService teamService, IMapper mapper, ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém times com paginação e pesquisa simples.
        /// </summary>
        /// <remarks>Retorna PagedResult contendo metadata (TotalCount, Page, PageSize).</remarks>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TeamDto>), 200)]
        public async Task<ActionResult<PagedResult<TeamDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 500) pageSize = 500;

            var result = await _teamService.GetPagedAsync(page, pageSize, q, cancellationToken);
            return Ok(_mapper.Map<PagedResult<TeamDto>>(result));
        }

        /// <summary>
        /// Obtém um time por id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TeamDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var team = await _teamService.GetByIdAsync(id, cancellationToken);
            if (team == null) return NotFound();
            return Ok(_mapper.Map<TeamDto>(team));
        }

        /// <summary>
        /// Cria um novo time. Owner será atribuído ao usuário autenticado.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TeamDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] TeamCreateDto input, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // extrai user id do token (ajuste claim se necessário)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var created = await _teamService.CreateAsync(input, userId, cancellationToken);
            _logger.LogInformation("Team created (Id={TeamId}) by User {UserId}", created.Id, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TeamDto>(created));
        }

        /// <summary>
        /// Atualiza um time — campos limitados.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] TeamUpdateDto input, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (id != input.Id) return BadRequest("ID mismatch");

            var updated = await _teamService.UpdateAsync(input, cancellationToken);
            if (!updated) return NotFound();
            _logger.LogInformation("Team updated (Id={TeamId})", id);
            return NoContent();
        }

        /// <summary>
        /// Remove um time.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _teamService.DeleteAsync(id, cancellationToken);
            if (!deleted) return NotFound();
            _logger.LogInformation("Team deleted (Id={TeamId})", id);
            return NoContent();
        }
    }
}