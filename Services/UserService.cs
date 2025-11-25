using Microsoft.EntityFrameworkCore;
using BFF_GameMatch.Services.Dtos.User;
using MyBffProject.Data;
using MyBffProject.Models;
using MyBffProject.Services.Results;
using MyBffProject.Services;

namespace BFF_GameMatch.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<UserDto>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct)
        {
            var query = _db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(u =>
                    (u.Name != null && u.Name.Contains(term)) ||
                    (u.Email != null && u.Email.Contains(term)));
            }

            var total = await query.CountAsync(ct);
            var users = await query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            var items = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CPF = u.CPF,
                Phone = u.Phone
            });

            return new PagedResult<UserDto>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var user = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);

            return user == null
                ? null
                : new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CPF = user.CPF,
                    Phone = user.Phone
                };
        }

        public async Task<UserDto> CreateAsync(UserCreateDto input, CancellationToken ct)
        {
            var user = new User
            {
                Name = input.Name,
                CPF = input.CPF,
                Phone = input.Phone,
                Email = input.Email,
                Password = input.Password
            };

            await _db.Users.AddAsync(user, ct);
            await _db.SaveChangesAsync(ct);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CPF = user.CPF,
                Phone = user.Phone
            };
        }

        public async Task<bool> UpdateAsync(UserUpdateDto input, CancellationToken ct)
        {
            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Id == input.Id, ct);
            if (existing == null) return false;

            existing.Name = input.Name ?? existing.Name;
            existing.Phone = input.Phone ?? existing.Phone;
            existing.Email = input.Email ?? existing.Email;
            if (!string.IsNullOrEmpty(input.Password))
                existing.Password = input.Password;

            _db.Users.Update(existing);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var existing = await _db.Users.FindAsync(new object[] { id }, ct);
            if (existing == null) return false;

            _db.Users.Remove(existing);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public Task<UserDto?> GetByIdAsync(string id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
