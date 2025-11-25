using AutoMapper;
using MyBffProject.Models;
using MyBffProject.Repositories;
using MyBffProject.Services.Results;
using MyBffProject.Services;

namespace BFF_GameMatch.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repo;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<PagedResult<Team>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct)
        {
            return _repo.GetPagedAsync(page, pageSize, q, ct);
        }

        public Task<Team?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.GetTeamByIdAsync(id, ct);

        public async Task<Team> CreateAsync(TeamCreateDto input, string? ownerUserId, CancellationToken ct)
        {
            var team = _mapper.Map<Team>(input);
            team.CreatedAt = DateTime.UtcNow;

            // garante compatibilidade: ownerUserId vem como string (ex: GUID ou Id numérico em texto)
            if (!string.IsNullOrEmpty(ownerUserId) && int.TryParse(ownerUserId, out var ownerId))
            {
                team.OwnerId = ownerId;
            }

            await _repo.CreateTeamAsync(team, ct);
            return team;
        }

        public async Task<bool> UpdateAsync(TeamUpdateDto input, CancellationToken ct)
        {
            var existing = await _repo.GetTeamByIdAsync(input.Id, ct);
            if (existing == null) return false;

            existing.Name = input.Name;
            existing.Description = input.Description;
            existing.SportType = input.SportType;
            existing.Address = input.Address;
            existing.Photo = input.Photo;

            await _repo.UpdateTeamAsync(existing, ct);
            return true;
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct) =>
            _repo.DeleteTeamAsync(id, ct);
    }
}
