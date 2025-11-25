using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyBffProject.Services.Dtos.Backend;

namespace MyBffProject.Services
{
    public class BackendProxyService : IBackendProxyService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<BackendProxyService> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public BackendProxyService(IHttpClientFactory httpFactory, ILogger<BackendProxyService> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        private HttpClient Client() => _httpFactory.CreateClient("backend");

        private async Task<T?> ReadAsJsonAsync<T>(HttpResponseMessage resp, CancellationToken cancellationToken)
        {
            if (resp.IsSuccessStatusCode)
                return await resp.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken);
            var txt = await resp.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Backend returned non-success: {Status} - {Body}", resp.StatusCode, txt);
            resp.EnsureSuccessStatusCode(); // will throw
            return default;
        }

        // Users
        public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            var resp = await Client().GetAsync("api/users", cancellationToken);
            return await ReadAsJsonAsync<List<UserDto>>(resp, cancellationToken) ?? new List<UserDto>();
        }

        public async Task<UserDto?> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var resp = await Client().GetAsync($"api/users/{id}", cancellationToken);
            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            return await ReadAsJsonAsync<UserDto>(resp, cancellationToken);
        }

        public async Task<UserDto> CreateUserAsync(UserDto user, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PostAsJsonAsync("api/users", user, _jsonOptions, cancellationToken);
            return await ReadAsJsonAsync<UserDto>(resp, cancellationToken) ?? throw new InvalidOperationException("No response body");
        }

        public async Task UpdateUserAsync(int id, UserDto user, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PutAsJsonAsync($"api/users/{id}", user, _jsonOptions, cancellationToken);
            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var resp = await Client().DeleteAsync($"api/users/{id}", cancellationToken);
            resp.EnsureSuccessStatusCode();
        }

        // Groups
        public async Task<List<GroupDto>> GetGroupsAsync(CancellationToken cancellationToken = default)
        {
            var resp = await Client().GetAsync("api/groups", cancellationToken);
            return await ReadAsJsonAsync<List<GroupDto>>(resp, cancellationToken) ?? new List<GroupDto>();
        }

        public async Task<GroupDto?> GetGroupAsync(int id, CancellationToken cancellationToken = default)
        {
            var resp = await Client().GetAsync($"api/groups/{id}", cancellationToken);
            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            return await ReadAsJsonAsync<GroupDto>(resp, cancellationToken);
        }

        public async Task<GroupDto> CreateGroupAsync(GroupCreateDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PostAsJsonAsync("api/groups", dto, _jsonOptions, cancellationToken);
            return await ReadAsJsonAsync<GroupDto>(resp, cancellationToken) ?? throw new InvalidOperationException("No response body");
        }

        public async Task UpdateGroupAsync(int id, GroupUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PutAsJsonAsync($"api/groups/{id}", dto, _jsonOptions, cancellationToken);
            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteGroupAsync(int id, CancellationToken cancellationToken = default)
        {
            var resp = await Client().DeleteAsync($"api/groups/{id}", cancellationToken);
            resp.EnsureSuccessStatusCode();
        }

        public async Task<GroupResponseDto> JoinGroupAsync(int id, JoinDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PostAsJsonAsync($"api/groups/{id}/addMembro", dto, _jsonOptions, cancellationToken);
            return await ReadAsJsonAsync<GroupResponseDto>(resp, cancellationToken) ?? throw new InvalidOperationException("No response body");
        }

        public async Task KickMemberAsync(int id, KickDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PostAsJsonAsync($"api/groups/{id}/remMembro", dto, _jsonOptions, cancellationToken);
            resp.EnsureSuccessStatusCode();
        }

        public async Task ReorderGroupAsync(int id, ReorderDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await Client().PostAsJsonAsync($"api/groups/{id}/reagrupar", dto, _jsonOptions, cancellationToken);
            resp.EnsureSuccessStatusCode();
        }
    }
}