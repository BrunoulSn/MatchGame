using AutoMapper;
using BFF_GameMatch.Services.Dtos.Group;
using BFF_GameMatch.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BFF_GameMatch.Controllers
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

        // GET: api/groups
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        // GET: api/groups/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        // POST: api/groups
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupCreateDto dto)
        {
            var createdGroup = await _groupService.CreateGroupAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdGroup.Id }, createdGroup);
        }

        // PUT: api/groups/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GroupUpdateDto dto)
        {
            var updatedGroup = await _groupService.UpdateGroupAsync(id, dto);
            if (updatedGroup == null) return NotFound();
            return NoContent();
        }

        // ✅ DELETE: api/groups/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _groupService.DeleteGroupAsync(id);
            return NoContent();
        }
    }
}
