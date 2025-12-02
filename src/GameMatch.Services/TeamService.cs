using GameMatch.Core.Models;
using GameMatch.Infrastructure.Repositories;

namespace GameMatch.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _repo;

        public TeamService(ITeamRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Team>> GetAllAsync(CancellationToken ct) =>
            _repo.GetAllAsync(ct);

        public Task<Team?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.GetByIdAsync(id, ct);

        public Task<Team> CreateAsync(Team team, CancellationToken ct) =>
            _repo.CreateAsync(team, ct);

        public Task<bool> UpdateAsync(Team team, CancellationToken ct) =>
            _repo.UpdateAsync(team, ct);

        public Task<bool> DeleteAsync(int id, CancellationToken ct) =>
            _repo.DeleteAsync(id, ct);
    }
}
