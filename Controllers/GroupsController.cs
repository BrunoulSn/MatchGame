using AutoMapper;
using BFF_GameMatch.Models;
using BFF_GameMatch.Services.Dtos.Group;
using BFF_GameMatch.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BFF_GameMatch.Controllers // CORRIGIDO: Namespace correto
{
    [ApiController]
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupsController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        // Retorna todos os grupos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups); // Já retorna List<GroupResponseDto>
        }

        // Retorna um grupo específico pelo ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();
            return Ok(group); // Já é GroupResponseDto
        }

        // Cria um novo grupo
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupCreateDto dto)
        {
            // CORRIGIDO: Passa diretamente o DTO para o serviço
            var createdGroup = await _groupService.CreateGroupAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdGroup.Id }, createdGroup);
        }

        // Atualiza um grupo existente
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GroupUpdateDto dto)
        {
            // CORRIGIDO: Passa id e DTO separadamente
            var updatedGroup = await _groupService.UpdateGroupAsync(id, dto);
            if (updatedGroup == null) return NotFound();
            return NoContent();
        }
    }
}