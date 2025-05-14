using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Users.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleOuputDto>> Create(RoleInsInputDto input);
        Task<BaseResponse<RoleOuputDto>> Update(string id, RoleInputDto input);
        Task<BaseResponse<RoleOuputDto>> GetById(string id);
        Task<BaseResponse<bool>> Delete(string id);
        Task<BaseResponse<List<RoleOuputDto>>> GetAllRole();
        Task<BaseResponse<RoloPermissionDto>> GetPermissionById(string roleId);
        Task<BaseResponse<RoloPermissionDto>> UpdatePermision(RoloPermissionDto input);
    }
}
