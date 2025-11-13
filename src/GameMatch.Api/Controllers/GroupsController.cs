// src/GameMatch.Api/Controllers/GroupsController.cs
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameMatch.Api.DTOs;

[ApiController]
[Route("api/groups")]
public class GroupsController : ControllerBase
{
    private readonly AppDb _db;
    public GroupsController(AppDb db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? sportId)
    {
        var q = _db.Groups
            .Include(g => g.Members).ThenInclude(m => m.User)
            .Include(g => g.Positions).ThenInclude(gp => gp.Position)
            .AsNoTracking();

        if (sportId.HasValue) q = q.Where(g => g.SportId == sportId.Value);
        return Ok(await q.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var g = await _db.Groups
            .Include(g => g.Members).ThenInclude(m => m.User)
            .Include(g => g.Positions).ThenInclude(gp => gp.Position)
            .FirstOrDefaultAsync(g => g.Id == id);
        return g is null ? NotFound() : Ok(g);
    }

    public record CreateGroupDto(string Name, string? Description, int SportId, int MaxMembers, int OwnerId);
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto dto)
    {
        if (!await _db.Sports.AnyAsync(s => s.Id == dto.SportId))
            return BadRequest("Sport inválido");

        var g = new Group
        {
            Name = dto.Name,
            Description = dto.Description,
            SportId = dto.SportId,
            MaxMembers = dto.MaxMembers,
            OwnerId = dto.OwnerId,
            CreatedAt = DateTime.UtcNow
        };
        _db.Groups.Add(g);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = g.Id }, g);
    }

    public record UpdateGroupDto(string Name, string? Description, int MaxMembers);
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGroupDto dto)
    {
        var g = await _db.Groups.FindAsync(id);
        if (g is null) return NotFound();
        g.Name = dto.Name;
        g.Description = dto.Description;
        g.MaxMembers = dto.MaxMembers;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var g = await _db.Groups.FindAsync(id);
        if (g is null) return NotFound();
        _db.Groups.Remove(g);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // RF08: entrar em patota (com posição opcional)
    public record JoinDto(int UserId, int? PositionId);

    [HttpPost("{id:int}/addMembro")]
    public async Task<IActionResult> Join(int id, [FromBody] JoinDto dto)
    {
        var g = await _db.Groups
            .Include(x => x.Members)
            .Include(x => x.Positions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (g is null)
            return NotFound();

        if (g.Members.Count >= g.MaxMembers)
            return BadRequest("Grupo cheio");

        if (dto.PositionId.HasValue)
        {
            var gp = g.Positions.FirstOrDefault(x => x.PositionId == dto.PositionId.Value);
            if (gp is null)
                return BadRequest("Posição não configurada no grupo");

            var ocupados = g.Members.Count(m => m.PositionId == gp.PositionId);
            if (ocupados >= gp.OpenSpots)
                return BadRequest("Sem vagas nessa posição");
        }

        var exists = g.Members.Any(m => m.UserId == dto.UserId);
        if (exists)
            return BadRequest("Usuário já está no grupo");

        _db.GroupMembers.Add(new GroupMember
        {
            GroupId = id,
            UserId = dto.UserId,
            PositionId = dto.PositionId ?? 0,
            Role = MemberRole.Member
        });

        await _db.SaveChangesAsync();

        var resp = new GroupResponseDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            SportId = g.SportId,
            MaxMembers = g.MaxMembers,
            OwnerId = g.OwnerId,
            CreatedAt = g.CreatedAt
        };

        return Ok(resp);
    }


    // RF09: remover membro (criador ou moderador)
    public record KickDto(int ActorId, int MemberUserId);
    [HttpPost("{id:int}/remMembro")]
    public async Task<IActionResult> Kick(int id, [FromBody] KickDto dto)
    {
        var g = await _db.Groups
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (g is null) return NotFound();

        var actor = g.Members.FirstOrDefault(m => m.UserId == dto.ActorId);
        if (actor is null || (g.OwnerId != dto.ActorId && actor.Role != MemberRole.Moderator))
            return Forbid();

        var victim = g.Members.FirstOrDefault(m => m.UserId == dto.MemberUserId);
        if (victim is null) return NotFound();

        _db.GroupMembers.Remove(victim);
        await _db.SaveChangesAsync();
        return Ok();
    }

    // RF12: reorganizar membros (drag & drop → mudar PositionId)
    public record ReorderItem(int UserId, int? PositionId);
    public record ReorderDto(List<ReorderItem> Items);
    [HttpPost("{id:int}/reagrupar")]
    public async Task<IActionResult> Reorder(int id, [FromBody] ReorderDto dto)
    {
        var g = await _db.Groups
            .Include(x => x.Members)
            .Include(x => x.Positions)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (g is null) return NotFound();

        foreach (var item in dto.Items)
        {
            var mem = g.Members.FirstOrDefault(m => m.UserId == item.UserId);
            if (mem is null) continue;
            if (item.PositionId.HasValue)
            {
                var gp = g.Positions.FirstOrDefault(x => x.PositionId == item.PositionId.Value);
                if (gp is null) return BadRequest($"Posição {item.PositionId} não está configurada no grupo");
                var ocupados = g.Members.Count(m => m.PositionId == gp.PositionId);
                if (ocupados >= gp.OpenSpots) return BadRequest("Sem vagas nessa posição");
                mem.PositionId = item.PositionId.Value;
            }
            else mem.PositionId = 0; // “sem posição”
        }
        await _db.SaveChangesAsync();
        return Ok();
    }
}
