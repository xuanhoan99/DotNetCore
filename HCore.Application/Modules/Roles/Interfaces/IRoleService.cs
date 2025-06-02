using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleOuputDto>> Create(RoleInsInputDto input);
        Task<BaseResponse<RoleOuputDto>> Update(string id, RoleInputDto input);
        Task<BaseResponse<RoleOuputDto>> GetById(string id);
        Task<BaseResponse<bool>> Delete(string id);
        Task<BaseResponse<List<RoleOuputDto>>> GetAllRole();
    }
}
