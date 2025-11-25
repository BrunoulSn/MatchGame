using System.Security.Claims;
using AutoMapper;
using BFF_GameMatch.Services.Dtos.Team;
using Microsoft.AspNetCore.Mvc;
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

        public TeamController(
            ITeamService teamService,
            IMapper mapper,
            ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retorna lista paginada de times com opção de busca.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TeamDto>), 200)]
        public async Task<ActionResult<PagedResult<TeamDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (page < 1) page = 1;
                if (page > 10_000) page = 10_000;
                if (pageSize < 1) pageSize = 20;
                if (pageSize > 500) pageSize = 500;

                var result = await _teamService.GetPagedAsync(page, pageSize, q, cancellationToken);
                return Ok(_mapper.Map<PagedResult<TeamDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar times paginados.");
                return StatusCode(500, new { mensagem = "Erro interno no servidor." });
            }
        }

        /// <summary>
        /// Obtém um time específico pelo ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TeamDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var team = await _teamService.GetByIdAsync(id, cancellationToken);
                if (team == null)
                    return NotFound(new { mensagem = $"Time {id} não encontrado." });

                return Ok(_mapper.Map<TeamDto>(team));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o time {TeamId}.", id);
                return StatusCode(500, new { mensagem = "Erro interno no servidor." });
            }
        }

        /// <summary>
        /// Cria um novo time. O usuário autenticado será definido como proprietário.
        /// </summary>
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(TeamDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(
            [FromBody] TeamCreateDto input,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (userId is null)
                {
                    _logger.LogWarning("Usuário autenticado sem claim de identificação.");
                    return Unauthorized(new { mensagem = "Não foi possível identificar o usuário autenticado." });
                }

                var created = await _teamService.CreateAsync(input, userId, cancellationToken);

                _logger.LogInformation("Time criado (Id={TeamId}) pelo usuário {UserId}.", created.Id, userId);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    _mapper.Map<TeamDto>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar time.");
                return StatusCode(500, new { mensagem = "Erro interno no servidor." });
            }
        }

        /// <summary>
        /// Atualiza dados de um time existente.
        /// </summary>
        [HttpPut("{id:int}")]
        //[Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] TeamUpdateDto input,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);
                if (id != input.Id)
                    return BadRequest(new { mensagem = "O ID da rota não corresponde ao ID do objeto enviado." });

                var updated = await _teamService.UpdateAsync(input, cancellationToken);

                if (!updated)
                    return NotFound(new { mensagem = $"Time {id} não encontrado." });

                _logger.LogInformation("Time atualizado (Id={TeamId}).", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o time {TeamId}.", id);
                return StatusCode(500, new { mensagem = "Erro interno no servidor." });
            }
        }

        /// <summary>
        /// Remove um time pelo ID.
        /// </summary>
        [HttpDelete("{id:int}")]
        //[Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await _teamService.DeleteAsync(id, cancellationToken);

                if (!deleted)
                    return NotFound(new { mensagem = $"Time {id} não encontrado." });

                _logger.LogInformation("Time removido (Id={TeamId}).", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover o time {TeamId}.", id);
                return StatusCode(500, new { mensagem = "Erro interno no servidor." });
            }
        }
    }
}
