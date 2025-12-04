using BFF_GameMatch.Services.Dtos.Group;

namespace BFF_GameMatch.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupResponseDto> GetGroupByIdAsync(int id);
        Task<GroupResponseDto> CreateGroupAsync(GroupCreateDto request); // Alterado de CreateGroupRequest para GroupCreateDto
        Task<GroupResponseDto> GetGroupAsync(int groupId);
        Task<List<GroupResponseDto>> GetAllGroupsAsync();
        Task<GroupResponseDto> UpdateGroupAsync(int groupId, GroupUpdateDto request); // Corrigido parâmetros
    }
}