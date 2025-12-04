using System.Net.Http.Json;
using BFF_GameMatch.Services.Interfaces;
using BFF_GameMatch.Services.Dtos.Group;
using Microsoft.Extensions.Logging;
using BFF_GameMatch.Models;

namespace BFF_GameMatch.Services
{
    public class GroupService : IGroupService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GroupService> _logger;

        public GroupService(HttpClient httpClient, ILogger<GroupService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<GroupResponseDto> GetGroupByIdAsync(int id)
        {
            return await GetGroupAsync(id);
        }

        public async Task<GroupResponseDto> CreateGroupAsync(GroupCreateDto request)
        {
            try
            {
                // O backend espera GroupCreateDto
                var response = await _httpClient.PostAsJsonAsync("api/groups", request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<GroupResponseDto>()
                    ?? throw new InvalidOperationException("Resposta vazia do backend");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar grupo");
                throw;
            }
        }

        public async Task<GroupResponseDto> GetGroupAsync(int groupId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GroupResponseDto>($"api/groups/{groupId}");
                return response ?? throw new InvalidOperationException("Grupo não encontrado");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Grupo {groupId} não encontrado");
            }
        }

        public async Task<List<GroupResponseDto>> GetAllGroupsAsync()
        {
            try
            {
                var groups = await _httpClient.GetFromJsonAsync<List<GroupResponseDto>>("api/groups");
                return groups ?? new List<GroupResponseDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar grupos");
                return new List<GroupResponseDto>();
            }
        }

        public async Task<GroupResponseDto> UpdateGroupAsync(int groupId, GroupUpdateDto request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/groups/{groupId}", request);
                response.EnsureSuccessStatusCode();

                // Retorna o grupo atualizado
                return await GetGroupAsync(groupId);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Grupo {groupId} não encontrado");
            }
        }

        // Métodos adicionais (opcional)
        public async Task<GroupResponseDto> AddMemberAsync(int groupId, int userId)
        {
            try
            {
                var joinDto = new { UserId = userId };
                var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/addMembro", joinDto);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<GroupResponseDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar membro ao grupo {GroupId}", groupId);
                throw;
            }
        }

        public async Task<bool> RemoveMemberAsync(int groupId, int actorId, int memberUserId)
        {
            try
            {
                var kickDto = new { ActorId = actorId, MemberUserId = memberUserId };
                var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/remMembro", kickDto);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover membro do grupo {GroupId}", groupId);
                return false;
            }
        }
    }
}