using System.Net.Http.Json;
using BFF_GameMatch.Services.Dtos;
using BFF_GameMatch.Services.Dtos.User;
using BFF_GameMatch.Services.Interfaces;
using BFF_GameMatch.Services.Results;

namespace BFF_GameMatch.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(HttpClient httpClient, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PagedResult<UserDto>> GetPagedAsync(
            int page, int pageSize, string? q, CancellationToken ct)
        {
            try
            {
                // Busca lista de usuários diretamente do backend
                var backendUsers = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users", ct);
                var allUsers = backendUsers ?? new List<UserDto>();

                // Filtro opcional
                if (!string.IsNullOrWhiteSpace(q))
                {
                    allUsers = allUsers
                        .Where(u =>
                            u.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                            u.Email.Contains(q, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Paginação manual
                var pagedItems = allUsers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PagedResult<UserDto>
                {
                    Items = pagedItems,
                    TotalCount = allUsers.Count,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários paginados");
                return new PagedResult<UserDto>();
            }
        }

        public async Task<UserDto?> GetByIdAsync(string id, CancellationToken ct)
        {
            try
            {
                var backendUser = await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{id}", ct);
                return backendUser;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário {UserId}", id);
                return null;
            }
        }

        public async Task<UserDto> CreateAsync(UserCreateDto input, CancellationToken ct)
        {
            try
            {
                // Monta o JSON igual ao esperado pelo backend
                var userData = new
                {
                    Name = input.Name,
                    Email = input.Email,
                    Password = input.Password,
                    Phone = input.Phone,
                    BirthDate = input.BirthDate,
                    Skills = input.Skills,
                    Availability = input.Availability
                };

                var response = await _httpClient.PostAsJsonAsync("api/users", userData, ct);
                response.EnsureSuccessStatusCode();

                var created = await response.Content.ReadFromJsonAsync<UserDto>(ct);
                return created ?? throw new InvalidOperationException("Resposta vazia do backend");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UserUpdateDto input, CancellationToken ct)
        {
            try
            {
                var userData = new
                {
                    Name = input.Name,
                    Email = input.Email,
                    Phone = input.Phone,
                    Password = input.Password,
                    BirthDate = input.BirthDate,
                    Skills = input.Skills,
                    Availability = input.Availability
                };

                var response = await _httpClient.PutAsJsonAsync($"api/users/{input.Id}", userData, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário {UserId}", input.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/users/{id}", ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário {UserId}", id);
                return false;
            }
        }
    }
}
