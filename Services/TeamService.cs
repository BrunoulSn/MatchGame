using System.Net.Http.Json;
using BFF_GameMatch.Services.Dtos.Team;
using BFF_GameMatch.Services.Interfaces;
using BFF_GameMatch.Services.Results;

namespace BFF_GameMatch.Services
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TeamService> _logger;

        public TeamService(HttpClient httpClient, ILogger<TeamService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PagedResult<TeamDto>> GetPagedAsync(
            int page, int pageSize, string? q, CancellationToken ct)
        {
            try
            {
                // O backend não tem paginação nativa, buscamos tudo e paginamos no BFF
                var teams = await _httpClient.GetFromJsonAsync<List<TeamDto>>("api/teams", ct);

                var allTeams = teams ?? new List<TeamDto>();

                // Aplicar filtro se houver
                if (!string.IsNullOrWhiteSpace(q))
                {
                    allTeams = allTeams.Where(t =>
                        t.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        (t.Description?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                // Paginação manual
                var pagedItems = allTeams
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PagedResult<TeamDto>
                {
                    Items = pagedItems,
                    TotalCount = allTeams.Count,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar times paginados");
                return new PagedResult<TeamDto>
                {
                    Items = new List<TeamDto>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }
        }

        public async Task<TeamDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<TeamDto>($"api/teams/{id}", ct);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<TeamDto> CreateAsync(TeamCreateDto input, string userId, CancellationToken ct)
        {
            try
            {
                // Criar objeto que o backend espera
                var teamData = new
                {
                    Name = input.Name,
                    Description = input.Description,
                    SportType = input.SportType,
                    OwnerId = int.Parse(userId), // Converter para int
                    Address = (string?)null,
                    Photo = (string?)null,
                    GroupId = 0 // Se necessário
                };

                var response = await _httpClient.PostAsJsonAsync("api/teams", teamData, ct);
                response.EnsureSuccessStatusCode();

                var created = await response.Content.ReadFromJsonAsync<TeamDto>(ct);
                return created ?? throw new InvalidOperationException("Resposta vazia do backend");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar time");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TeamUpdateDto input, CancellationToken ct)
        {
            try
            {
                var teamData = new
                {
                    Name = input.Name,
                    Description = input.Description,
                    SportType = input.SportType,
                    Address = input.Address,
                    Photo = input.Photo
                };

                var response = await _httpClient.PutAsJsonAsync($"api/teams/{input.Id}", teamData, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar time {TeamId}", input.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/teams/{id}", ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar time {TeamId}", id);
                return false;
            }
        }
    }
}