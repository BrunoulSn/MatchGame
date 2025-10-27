using Microsoft.EntityFrameworkCore;
using MyBffProject.Data;
using MyBffProject.Models;
using MyBffProject.Services.Results;

namespace MyBffProject.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams
                .Include(t => t.Owner)           // Inclui o dono
                //.Include(t => t.Members)      // Inclui membros, se existir no modelo
                .ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.Owner)
                //.Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }

        public Task<PagedResult<Team>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Team?> GetTeamByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task CreateTeamAsync(Team team, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTeamAsync(Team team, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTeamAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
