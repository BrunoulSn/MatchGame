using GameMatch.Core.Models;
using GameMatch.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameMatch.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _repo;

        public TeamService(ITeamRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Team team)
        {
            await _repo.AddAsync(team);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            await _repo.UpdateAsync(team);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
        }
    }
}
