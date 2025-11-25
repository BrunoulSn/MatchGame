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
    public async Task<IActionResult> List()
    {
        var q = _db.Groups
            .AsNoTracking();
        return Ok(await q.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var g = await _db.Groups
            .FirstOrDefaultAsync(g => g.Id == id);
        return g is null ? NotFound() : Ok(g);
    }

    //public record CreateGroupDto(string Name, string? Description, int OwnerId);
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupCreateDto dto)
    {

        var g = new Group
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId,
            CreatedAt = DateTime.UtcNow
        };
        _db.Groups.Add(g);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = g.Id }, g);
    }

    //public record UpdateGroupDto(string Name, string? Description);
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] GroupUpdateDto dto)
    {
        var g = await _db.Groups.FindAsync(id);
        if (g is null) return NotFound();
        g.Name = dto.Name;
        g.Description = dto.Description;
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
    public record JoinDto(int UserId);

    [HttpPost("{id:int}/addMembro")]
    public async Task<IActionResult> Join(int id, [FromBody] JoinDto dto)
    {
        var g = await _db.Groups
            .FirstOrDefaultAsync(x => x.Id == id);

        if (g is null)
            return NotFound();

        _db.GroupMembers.Add(new GroupMember
        {
            GroupId = id,
            UserId = dto.UserId,
            Role = MemberRole.Member
        });

        await _db.SaveChangesAsync();

        var resp = new GroupResponseDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
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
            .FirstOrDefaultAsync(x => x.Id == id);
        if (g is null) return NotFound();

        await _db.SaveChangesAsync();
        return Ok();
    }

    // RF12: reorganizar membros (drag & drop → mudar PositionId)
    public record ReorderItem(int UserId);
    public record ReorderDto(List<ReorderItem> Items);
    [HttpPost("{id:int}/reagrupar")]
    public async Task<IActionResult> Reorder(int id, [FromBody] ReorderDto dto)
    {
        var g = await _db.Groups
            .FirstOrDefaultAsync(x => x.Id == id);
        if (g is null) return NotFound();

        await _db.SaveChangesAsync();
        return Ok();
    }
}
