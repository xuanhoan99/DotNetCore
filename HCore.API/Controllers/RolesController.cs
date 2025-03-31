using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleInsInputDto role)
        {
            var result = await _roleService.RoleIns(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), role);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _roleService.RoleById(id);
            if (result == null)
            {
                return NotFound(new { message = "Role not found" });
            }
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RoleInputDto role)
        {
            var result = await _roleService.RoleUpd(role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleService.RoleDel(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllRole();
            return Ok(result);
        }
    }
}
