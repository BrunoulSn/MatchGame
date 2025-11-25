using MyBffProject.Models;
using MyBffProject.Services.Results;
using System.Threading;

namespace MyBffProject.Services
{
    public interface ITeamService
    {
        Task<PagedResult<Team>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
        Task<Team?> GetByIdAsync(int id, CancellationToken ct);
        Task<Team> CreateAsync(TeamCreateDto input, string? ownerUserId, CancellationToken ct);
        Task<bool> UpdateAsync(TeamUpdateDto input, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}