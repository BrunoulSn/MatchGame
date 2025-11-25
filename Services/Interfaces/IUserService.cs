using System.Threading;
using System.Threading.Tasks;
using BFF_GameMatch.Models;
using BFF_GameMatch.Services.Dtos.User;
using MyBffProject.Services.Results;

namespace MyBffProject.Services
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
        Task<User?> GetByIdAsync(string id, CancellationToken ct);
        Task<User> CreateAsync(UserCreateDto input, CancellationToken ct);
        Task<bool> UpdateAsync(UserUpdateDto input, CancellationToken ct);
        Task<bool> DeleteAsync(string id, CancellationToken ct);
    }
}   