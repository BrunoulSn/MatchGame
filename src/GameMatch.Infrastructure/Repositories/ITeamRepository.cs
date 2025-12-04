using GameMatch.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameMatch.Infrastructure.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
