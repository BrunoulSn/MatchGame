using GameMatch.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameMatch.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDb _db;
        public TeamRepository(AppDb db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Team>> GetAllAsync() => await _db.Teams.ToListAsync();
        public async Task<Team?> GetByIdAsync(int id) => await _db.Teams.FindAsync(id);
        public async Task AddAsync(Team team) => await _db.Teams.AddAsync(team);
        public async Task UpdateAsync(Team team) => _db.Teams.Update(team);
        public async Task DeleteAsync(int id)
        {
            var t = await _db.Teams.FindAsync(id);
            if (t != null) _db.Teams.Remove(t);
        }
        public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}
