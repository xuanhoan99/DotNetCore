using HCore.Application.Modules.Roles.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> RoleIns(RoleInsInputDto input);
        Task<IdentityResult> RoleUpd(RoleInputDto input);
        Task<RoleOuputDto> RoleById(string id);
        Task<IdentityResult> RoleDel(string id);
        Task<List<RoleOuputDto>> GetAllRole();
    }
}
