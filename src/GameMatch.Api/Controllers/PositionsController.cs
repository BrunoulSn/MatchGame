using GameMatch.Api.DTOs;
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
    public async Task<ActionResult<IEnumerable<PositionDto>>> List()
    {
        var positions = await _db.Positions
            .AsNoTracking()
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Name = p.Name,
                Characteristics = p.Characteristics,
                SportId = p.SportId
            })
            .ToListAsync();

        return Ok(positions);
    }

   
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PositionDto>> Get(int id)
    {
        var p = await _db.Positions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p is null) return NotFound();

        var dto = new PositionDto
        {
            Id = p.Id,
            Name = p.Name,
            Characteristics = p.Characteristics,
            SportId = p.SportId
        };

        return Ok(dto);
    }

    
    [HttpPost]
    public async Task<ActionResult<PositionDto>> Create([FromBody] CreatePositionDto dto)
    {
        
        var sportExists = await _db.Sports.AnyAsync(s => s.Id == dto.SportId);
        if (!sportExists)
            return BadRequest("Sport inválido");

        var p = new Position
        {
            Name = dto.Name,
            Characteristics = dto.Characteristics,
            SportId = dto.SportId
        };

        _db.Positions.Add(p);
        await _db.SaveChangesAsync();

        var resp = new PositionDto
        {
            Id = p.Id,
            Name = p.Name,
            Characteristics = p.Characteristics,
            SportId = p.SportId
        };

        return CreatedAtAction(nameof(Get), new { id = p.Id }, resp);
    }

   
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePositionDto dto)
    {
        var p = await _db.Positions.FindAsync(id);
        if (p is null) return NotFound();

        p.Name = dto.Name;
        p.Characteristics = dto.Characteristics;

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
