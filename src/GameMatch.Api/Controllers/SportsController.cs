// src/GameMatch.Api/Controllers/SportsController.cs
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Create([FromBody] Sport sport)
    {
        _db.Sports.Add(sport);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = sport.Id }, sport);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Sport dto)
    {
        var s = await _db.Sports.FindAsync(id);
        if (s is null) return NotFound();
        s.Name = dto.Name;
        s.IconUrl = dto.IconUrl;
        s.Description = dto.Description;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Sports.FindAsync(id);
        if (s is null) return NotFound();
        _db.Sports.Remove(s);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}