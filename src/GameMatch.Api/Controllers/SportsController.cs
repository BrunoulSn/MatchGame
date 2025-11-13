
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameMatch.Api.DTOs;


[ApiController]
[Route("api/sports")]
public class SportsController : ControllerBase
{
    private readonly AppDb _db;
    public SportsController(AppDb db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Sports
            .Include(s => s.Positions)
            .AsNoTracking()
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var sport = await _db.Sports
            .Include(s => s.Positions)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sport is null ? NotFound() : Ok(sport);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SportDto dto)
    {
        var sport = new Sport
        {
            Name = dto.Name,
            IconUrl = dto.IconUrl,
            Description = dto.Description
        };

        _db.Sports.Add(sport);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = sport.Id }, new SportResponseDto
        {
            Id = sport.Id,
            Name = sport.Name,
            IconUrl = sport.IconUrl,
            Description = sport.Description
        });

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SportUpdateDto dto)
    {
        var sport = await _db.Sports.FindAsync(id);
        if (sport is null) return NotFound();

        sport.Name = dto.Name;
        sport.IconUrl = dto.IconUrl;
        sport.Description = dto.Description;

        await _db.SaveChangesAsync();
        return Ok(new SportResponseDto
        {
            Id = sport.Id,
            Name = sport.Name,
            IconUrl = sport.IconUrl,
            Description = sport.Description
        });

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sport = await _db.Sports.FindAsync(id);
        if (sport is null) return NotFound();

        _db.Sports.Remove(sport);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
