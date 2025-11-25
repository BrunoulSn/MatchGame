using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BFF_GameMatch.Services.Dtos.Team;
using MyBffProject.Services.Dtos.Backend;
using BFF_GameMatch.Services.Dtos.User;


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
        Task<List<BackGroupDto>> GetGroupsAsync(CancellationToken cancellationToken = default);
        Task<BackGroupDto?> GetGroupAsync(int id, CancellationToken cancellationToken = default);
        Task<BackGroupDto> CreateGroupAsync(BackGroupCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateGroupAsync(int id, BackGroupUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteGroupAsync(int id, CancellationToken cancellationToken = default);

        Task<BackGroupResponseDto> JoinGroupAsync(int id, BackJoinDto dto, CancellationToken cancellationToken = default);
        Task KickMemberAsync(int id, BackKickDto dto, CancellationToken cancellationToken = default);
        Task ReorderGroupAsync(int id, BackReorderItem dto, CancellationToken cancellationToken = default);
        Task UpdateGroupAsync(int id, TeamUpdateDto dto, CancellationToken cancellationToken);
        Task CreateGroupAsync(CreateGroupRequest dto, CancellationToken cancellationToken);
    }
}