using Microsoft.AspNetCore.Mvc;

namespace BFF_GameMatch.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class TeamController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return StatusCode(501, new { message = "Teams endpoint está desabilitado temporariamente. Use Groups." });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return StatusCode(501, new { message = "Teams endpoint está desabilitado temporariamente. Use Groups." });
        }

        [HttpPost]
        public IActionResult Create()
        {
            return StatusCode(501, new { message = "Teams endpoint está desabilitado temporariamente. Use Groups." });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id)
        {
            return StatusCode(501, new { message = "Teams endpoint está desabilitado temporariamente. Use Groups." });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(501, new { message = "Teams endpoint está desabilitado temporariamente. Use Groups." });
        }
    }
}