
// src/GameMatch.Api/Controllers/PositionsController.cs
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/positions")]
public class PositionsController : ControllerBase
{
    private readonly AppDb _db;
    public PositionsController(AppDb db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Positions.AsNoTracking().ToListAsync());

    [HttpGet("by-sport/{sportId:int}")]
    public async Task<IActionResult> BySport(int sportId) =>
        Ok(await _db.Positions.Where(p => p.SportId == sportId).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _db.Positions.FindAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Position p)
    {
        _db.Positions.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Position dto)
    {
        var p = await _db.Positions.FindAsync(id);
        if (p is null) return NotFound();
        p.Name = dto.Name;
        p.Characteristics = dto.Characteristics;
        p.SportId = dto.SportId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Positions.FindAsync(id);
        if (p is null) return NotFound();
        _db.Positions.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}