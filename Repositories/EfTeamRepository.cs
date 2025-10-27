using Microsoft.EntityFrameworkCore;
using MyBffProject.Data;
using MyBffProject.Models;
using MyBffProject.Services.Results;

namespace MyBffProject.Repositories
{
    public class EfTeamRepository : ITeamRepository
    {
        private readonly AppDbContext _db;

        public EfTeamRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<Team>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct)
        {
            var query = _db.Teams
                .AsNoTracking()
                .Include(t => t.Owner)
                .Include(t => t.Members)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(t => EF.Functions.Like(t.Name, $"%{q}%"));
            }

            var total = await query.LongCountAsync(ct).ConfigureAwait(false);

            var items = await query
                .OrderBy(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return new PagedResult<Team>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken ct)
        {
            return await _db.Teams
                .AsNoTracking()
                .Include(t => t.Owner)
                .Include(t => t.Members)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<Team?> GetTeamByIdAsync(int id, CancellationToken ct)
        {
            return await _db.Teams
                .AsNoTracking()
                .Include(t => t.Owner)
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id, ct)
                .ConfigureAwait(false);
        }

        public async Task CreateTeamAsync(Team team, CancellationToken ct)
        {
            await _db.Teams.AddAsync(team, ct).ConfigureAwait(false);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UpdateTeamAsync(Team team, CancellationToken ct)
        {
            _db.Teams.Update(team);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task<bool> DeleteTeamAsync(int id, CancellationToken ct)
        {
            var existing = await _db.Teams.FindAsync(new object[] { id }, ct).ConfigureAwait(false);
            if (existing == null) return false;
            _db.Teams.Remove(existing);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
    }
}