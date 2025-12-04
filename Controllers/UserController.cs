using AutoMapper;
using BFF_GameMatch.Services.Dtos.User;
using BFF_GameMatch.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BFF_GameMatch.Services.Results;
using System.Net.Http.Json;

namespace MyBffProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly HttpClient _http;

        public UserController(
            IUserService userService,
            IMapper mapper,
            ILogger<UserController> logger,
            IHttpClientFactory factory)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _http = factory.CreateClient("GameMatchApi");
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<UserDto>), 200)]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 500) pageSize = 500;

            var result = await _userService.GetPagedAsync(page, pageSize, q, cancellationToken);
            return Ok(_mapper.Map<PagedResult<UserDto>>(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetById(string id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UserCreateDto input, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var created = await _userService.CreateAsync(input, cancellationToken);
            _logger.LogInformation("User created (Id={UserId}, Email={Email})", created.Id, created.Email);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto input, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (input.Id != int.Parse(id))
                return BadRequest("ID mismatch");

            var updated = await _userService.UpdateAsync(input, cancellationToken);
            if (!updated) return NotFound();

            _logger.LogInformation("User updated (Id={UserId})", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            var deleted = await _userService.DeleteAsync(id, cancellationToken);
            if (!deleted) return NotFound();

            _logger.LogInformation("User deleted (Id={UserId})", id);
            return NoContent();
        }

        // 🔹 Login simples usando Email + Password
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BffLoginRequest dto)
        {
            var response = await _http.GetAsync("/api/users/login");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erro ao acessar backend");

            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            var user = users?.FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos");

            return Ok(new
            {
                message = "Login realizado com sucesso",
                user
            });
        }

        // classe separada para evitar conflito com o backend
        public class BffLoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    } // <-- essa fecha a classe UserController

    public class BffLoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }


        public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
