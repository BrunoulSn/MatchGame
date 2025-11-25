using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BFF_GameMatch.Models;
using MyBffProject.Data;
using MyBffProject.Services.Results;
using Microsoft.EntityFrameworkCore;
using BFF_GameMatch.Services.Dtos.User;

namespace MyBffProject.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct)
        {
            var query = _db.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(u => (u.Name != null && u.Name.Contains(term)) || (u.Email != null && u.Email.Contains(term)));
            }

            var total = await query.LongCountAsync(ct).ConfigureAwait(false);

            var items = await query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return new PagedResult<User>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<User?> GetByIdAsync(string id, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct).ConfigureAwait(false);
        }

        public async Task<User> CreateAsync(UserCreateDto input, CancellationToken ct)
        {
            var user = new User
            {
                Id = string.IsNullOrWhiteSpace(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                Name = input.Name,
                CPF = input.CPF,
                Phone = input.Phone,
                Email = input.Email,
                Password = input.Password
            };

            await _db.Users.AddAsync(user, ct).ConfigureAwait(false);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return user;
        }

        public async Task<bool> UpdateAsync(UserUpdateDto input, CancellationToken ct)
        {
            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Id == input.Id, ct).ConfigureAwait(false);
            if (existing == null) return false;

            existing.Name = input.Name ?? existing.Name;
            existing.Phone = input.Phone ?? existing.Phone;
            existing.Email = input.Email ?? existing.Email;
            if (!string.IsNullOrEmpty(input.Password)) existing.Password = input.Password;

            _db.Users.Update(existing);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken ct)
        {
            var existing = await _db.Users.FindAsync(new object[] { id }, ct).ConfigureAwait(false);
            if (existing == null) return false;
            _db.Users.Remove(existing);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
    }
}
