using GameMatch.Api.DTOs;
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDb _db;
    public UsersController(AppDb db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var users = await _db.Users
            .AsNoTracking()
            .Select(u => new {
                u.Id,
                u.Name,
                u.Email,
                u.Phone,
                u.BirthDate,
                u.Skills,
                u.Availability
            })
            .ToListAsync(); // ⚠️ Agora funciona com o using correto

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();

        return Ok(new
        {
            u.Id,
            u.Name,
            u.Email,
            u.Phone,
            u.BirthDate,
            u.Skills,
            u.Availability
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (string.IsNullOrEmpty(user.Password))
            return BadRequest(new { error = "Password é obrigatória" });

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = user.Id }, new
        {
            user.Id,
            user.Name,
            user.Email,
            user.Phone,
            user.BirthDate,
            user.Skills,
            user.Availability
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User dto)
    {
        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();

        u.Name = dto.Name;
        u.Email = dto.Email;
        u.Phone = dto.Phone;
        u.Password = dto.Password; // ⚠️ Agora é Password, não PasswordHash
        u.BirthDate = dto.BirthDate;
        u.Skills = dto.Skills;
        u.Availability = dto.Availability;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();

        _db.Users.Remove(u);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("login")]
    public async Task<IActionResult> GetUsersWithPassword()
    {
        var users = await _db.Users.ToListAsync();
        var userDtos = users.Select(u => new UserLoginDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Password = u.Password // 🔹 devolve a senha
        }).ToList();

        return Ok(userDtos);
    }

}