using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Permissions.Dtos;

namespace HCore.Application.Modules.Permissions.Interfaces
{
    public interface IRolePermissionService
    {
        Task<BaseResponse<RolePermissionDto>> GetPermissionById(string roleId);
        Task<BaseResponse<RolePermissionDto>> UpdatePermision(RolePermissionDto input);
        Task<BaseResponse<RolePermissionDto>> GetPermissionAll();
    }
}
