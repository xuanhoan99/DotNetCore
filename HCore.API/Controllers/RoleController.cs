using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Admin")]  // Chỉ Admin mới có quyền truy cập các endpoint quản lý role
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            var role = await _roleService.CreateRoleAsync(dto);
            return Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] CreateRoleDto dto)
        {
            var result = await _roleService.UpdateRoleAsync(id, dto);
            return result ? Ok("Role updated successfully") : NotFound("Role not found");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return result ? Ok("Role deleted successfully") : NotFound("Role not found");
        }
    }
}
