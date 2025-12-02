using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameMatch.Infrastructure;


namespace GameMatch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TeamsController : ControllerBase
    {
        private readonly AppDb _db;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(AppDb db, ILogger<TeamsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var teams = await _db.Teams.AsNoTracking().ToListAsync(ct);
            return Ok(teams);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var team = await _db.Teams.FindAsync(new object[] { id }, ct);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Team team, CancellationToken ct)
        {
            team.CreatedAt = DateTime.UtcNow;
            _db.Teams.Add(team);
            await _db.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Team input, CancellationToken ct)
        {
            var team = await _db.Teams.FindAsync(new object[] { id }, ct);
            if (team == null) return NotFound();

            team.Name = input.Name;
            team.Description = input.Description;
            team.SportType = input.SportType;
            team.Address = input.Address;
            team.Photo = input.Photo;

            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var team = await _db.Teams.FindAsync(new object[] { id }, ct);
            if (team == null) return NotFound();

            _db.Teams.Remove(team);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
