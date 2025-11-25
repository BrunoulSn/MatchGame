using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyBffProject.Services.Dtos.Backend;

namespace MyBffProject.Services
{
    public interface IBackendProxyService
    {
        // Users
        Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserAsync(int id, CancellationToken cancellationToken = default);
        Task<UserDto> CreateUserAsync(UserDto user, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(int id, UserDto user, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);

        // Groups
        Task<List<GroupDto>> GetGroupsAsync(CancellationToken cancellationToken = default);
        Task<GroupDto?> GetGroupAsync(int id, CancellationToken cancellationToken = default);
        Task<GroupDto> CreateGroupAsync(GroupCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateGroupAsync(int id, GroupUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteGroupAsync(int id, CancellationToken cancellationToken = default);

        Task<GroupResponseDto> JoinGroupAsync(int id, JoinDto dto, CancellationToken cancellationToken = default);
        Task KickMemberAsync(int id, KickDto dto, CancellationToken cancellationToken = default);
        Task ReorderGroupAsync(int id, ReorderDto dto, CancellationToken cancellationToken = default);
    }
}