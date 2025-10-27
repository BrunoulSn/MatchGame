using AutoMapper;
using BFF_GameMatch.Models;
using BFF_GameMatch.Services.Dtos.Team;
using MyBffProject.Models;
using MyBffProject.Repositories;
using MyBffProject.Services.Results;

namespace MyBffProject.Services
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
            // delega para o repositório que executará paginação no banco
            return _repo.GetPagedAsync(page, pageSize, q, ct);
        }

        public Task<Team?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.GetTeamByIdAsync(id, ct);

        public async Task<Team> CreateAsync(TeamCreateDto input, string? ownerUserId, CancellationToken ct)
        {
            var team = _mapper.Map<Team>(input);
            team.CreatedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(ownerUserId))
            {
                team.Owner = new User { Id = ownerUserId }; // ajustar se precisar buscar full user
            }
            await _repo.CreateTeamAsync(team, ct);
            return team;
        }

        public async Task<bool> UpdateAsync(TeamUpdateDto input, CancellationToken ct)
        {
            var existing = await _repo.GetTeamByIdAsync(input.Id, ct);
            if (existing == null) return false;
            // mapear apenas campos permitidos
            existing.Name = input.Name;
            existing.Description = input.Description;
            existing.SportType = input.SportType;
            existing.Address = input.Address;
            existing.Photo = input.Photo;
            await _repo.UpdateTeamAsync(existing, ct);
            return true;
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct) => _repo.DeleteTeamAsync(id, ct);

        public Task<bool> UpdateAsync(TeamDto input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(TeamDto input, string? userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Services.Dtos.TeamUpdateDto input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Services.Dtos.TeamCreateDto input, string? userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Services.Dtos.TeamUpdateDto input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Services.Dtos.TeamCreateDto input, string? userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}