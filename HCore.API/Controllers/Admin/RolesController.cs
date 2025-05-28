using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
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
        public async Task<BaseResponse<RoleOuputDto>> Create([FromBody] RoleInsInputDto roleInsInputDto)
        {
            var result = await _roleService.Create(roleInsInputDto);
            return result;

        }
        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.View)]
        public async Task<BaseResponse<RoleOuputDto>> GetById(string id)
        {
            var result = await _roleService.GetById(id);
            return result;
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Update)]
        public async Task<BaseResponse<RoleOuputDto>> Update(string id, [FromBody] RoleInputDto role)
        {
            var result = await _roleService.Update(id, role);
            return result;

        }
        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Delete)]
        public async Task<BaseResponse<bool>> Delete(string id)
        {
            var result = await _roleService.Delete(id);
            return result;
        }
        [HttpGet("all")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Search)]
        public async Task<BaseResponse<List<RoleOuputDto>>> GetAll()
        {
            var result = await _roleService.GetAllRole();
            return result;
        }
        [HttpGet("Permission/{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.View)]
        public async Task<BaseResponse<RoloPermissionDto>> GetPermissionById(string id)
        {
            var result = await _roleService.GetPermissionById(id);
            return result;
        }
        [HttpPut("Permission")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.Role, HCorePermissions.Action.Update)]
        public async Task<BaseResponse<RoloPermissionDto>> UpdatePermision([FromBody] RoloPermissionDto input)
        {
            var result = await _roleService.UpdatePermision(input);
            return result;
        }
    }
}
