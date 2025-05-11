using HCore.Application.Modules.Common;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.SysMenus.Dtos;
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
        public async Task<IActionResult> Create([FromBody] RoleInsInputDto roleInsInputDto)
        {
            var role = await _roleService.RoleIns(roleInsInputDto);

            if (role == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = role!.Id }, role);

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
            var result = await _roleService.RoleUpd(id, role);
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
        [HttpGet("Permission/{id}")]
        [ProducesResponseType(typeof(BaseResponse<RoloPermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<RoloPermissionDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPermissionById(string id)
        {
            var result = await _roleService.GetPermissionById(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPut("Permission")]
        [ProducesResponseType(typeof(BaseResponse<RoloPermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<RoloPermissionDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePermision([FromBody] RoloPermissionDto input)
        {
            var result = await _roleService.UpdatePermision(input);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
