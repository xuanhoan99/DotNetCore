using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleOutputDto>> Create(RoleInsInputDto input);
        Task<BaseResponse<RoleOutputDto>> Update(string id, RoleInputDto input);
        Task<BaseResponse<RoleOutputDto>> GetById(string id);
        Task<BaseResponse<bool>> Delete(string id);
        Task<BaseResponse<List<RoleOutputDto>>> GetAllRole();
    }
}
