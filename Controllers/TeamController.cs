using Microsoft.AspNetCore.Mvc;
using MyBffProject.Models;
using MyBffProject.Repositories;

namespace MyBffProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var team = await _teamRepository.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Team team)
        {
            await _teamRepository.CreateTeamAsync(team);
            return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Team team)
        {
            if (id != team.Id) return BadRequest("ID mismatch");
            await _teamRepository.UpdateTeamAsync(team);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamRepository.DeleteTeamAsync(id);
            return NoContent();
        }
    }
}
