using System.Threading;
using System.Threading.Tasks;
using MyBffProject.Models;
using MyBffProject.Services.Results;

namespace MyBffProject.Repositories
{
    public interface ITeamRepository
    {
        // Paginação executada no banco (eficiente)
        Task<PagedResult<Team>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
        Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken ct);
        Task<Team?> GetTeamByIdAsync(int id, CancellationToken ct);
        Task CreateTeamAsync(Team team, CancellationToken ct);
        Task UpdateTeamAsync(Team team, CancellationToken ct);
        Task<bool> DeleteTeamAsync(int id, CancellationToken ct);
    }
}
