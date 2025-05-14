using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.Users.Dtos;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers.Admin
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
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Create)]
        public async Task<IActionResult> Create([FromBody] RoleInsInputDto roleInsInputDto)
        {
            var result = await _roleService.Create(roleInsInputDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);

        }
        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.View)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _roleService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Update)]
        public async Task<IActionResult> Update(string id, [FromBody] RoleInputDto role)
        {
            var result = await _roleService.Update(id, role);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }
        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleService.Delete(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("all")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Search)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllRole();
            return Ok(result);
        }
        [HttpGet("Permission/{id}")]
        //[HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.View)]
        public async Task<IActionResult> GetPermissionById(string id)
        {
            var result = await _roleService.GetPermissionById(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPut("Permission")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Update)]
        public async Task<IActionResult> UpdatePermision([FromBody] RoloPermissionDto input)
        {
            var result = await _roleService.UpdatePermision(input);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
