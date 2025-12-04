using BFF_GameMatch.Services.Dtos.User;
using BFF_GameMatch.Services.Results;

namespace BFF_GameMatch.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
        Task<UserDto?> GetByIdAsync(string id, CancellationToken ct);
        Task<UserDto> CreateAsync(UserCreateDto input, CancellationToken ct);
        Task<bool> UpdateAsync(UserUpdateDto input, CancellationToken ct);
        Task<bool> DeleteAsync(string id, CancellationToken ct);
    }
}
