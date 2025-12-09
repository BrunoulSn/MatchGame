using BFF_GameMatch.Services.Dtos.Group;

namespace BFF_GameMatch.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupResponseDto> GetGroupByIdAsync(int id);
        Task<GroupResponseDto> CreateGroupAsync(GroupCreateDto request);
        Task<GroupResponseDto> GetGroupAsync(int groupId);
        Task<List<GroupResponseDto>> GetAllGroupsAsync();
        Task<GroupResponseDto> UpdateGroupAsync(int groupId, GroupUpdateDto request);

        Task DeleteGroupAsync(int id);
    }
}
