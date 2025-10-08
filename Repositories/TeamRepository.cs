using Microsoft.EntityFrameworkCore;
using MyBffProject.Data;
using MyBffProject.Models;

namespace MyBffProject.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task CreateTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            //await _context.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                //await _context.SaveChangesAsync();
            }
        }
    }
}
