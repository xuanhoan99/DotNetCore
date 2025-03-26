using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost]
        public async Task<IActionResult> RoleIns([FromBody] RoleInsInputDto role)
        {
            var result = await _roleService.RoleIns(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(RoleById), role);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        [HttpGet]
        public async Task<IActionResult> RoleById(string id)
        {
            var result = await _roleService.RoleById(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> RoleUpd([FromBody] RoleInputDto role)
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
        [HttpDelete]
        public async Task<IActionResult> RoleDel(string id)
        {
            var result = await _roleService.RoleDel(id);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            var result = await _roleService.GetAllRole();
            return Ok(result);
        }
    }
}
