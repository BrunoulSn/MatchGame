using BFF_GameMatch.Services.Dtos.Team;
using BFF_GameMatch.Services.Results;

namespace BFF_GameMatch.Services.Interfaces

{
    public interface ITeamService
    {
        Task<PagedResult<TeamDto>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
        Task<TeamDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<TeamDto> CreateAsync(TeamCreateDto input, string userId, CancellationToken ct);
        Task<bool> UpdateAsync(TeamUpdateDto input, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
