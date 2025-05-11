using HCore.Application.Modules.Common;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.SysMenus.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<RoleOuputDto> RoleIns(RoleInsInputDto input);
        Task<IdentityResult> RoleUpd(string id, RoleInputDto input);
        Task<RoleOuputDto> RoleById(string id);
        Task<IdentityResult> RoleDel(string id);
        Task<List<RoleOuputDto>> GetAllRole();
        Task<BaseResponse<RoloPermissionDto>> GetPermissionById(string roleId);
        Task<BaseResponse<RoloPermissionDto>> UpdatePermision(RoloPermissionDto input);
    }
}
