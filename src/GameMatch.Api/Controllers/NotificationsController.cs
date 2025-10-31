// src/GameMatch.Api/Controllers/NotificationsController.cs
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly AppDb _db;
    public NotificationsController(AppDb db) => _db = db;

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> ByUser(int userId) =>
        Ok(await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync());

    [HttpPost("read/{id:int}")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var n = await _db.Notifications.FindAsync(id);
        if (n is null) return NotFound();
        n.IsRead = true;
        await _db.SaveChangesAsync();
        return Ok();
    }
}
