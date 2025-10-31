// src/GameMatch.Api/Controllers/PostsController.cs
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly AppDb _db;
    public PostsController(AppDb db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Posts.Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt).ToListAsync());

    [HttpGet("feed")]
    public async Task<IActionResult> Feed() =>
        Ok(await _db.Posts.Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt).ToListAsync());

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> ByUser(int userId) =>
        Ok(await _db.Posts.Include(p => p.User)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _db.Posts.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Post post)
    {
        post.CreatedAt = DateTime.UtcNow;
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Post dto)
    {
        var p = await _db.Posts.FindAsync(id);
        if (p is null) return NotFound();
        p.Caption = dto.Caption;
        p.ImageUrl = dto.ImageUrl;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Posts.FindAsync(id);
        if (p is null) return NotFound();
        _db.Posts.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
