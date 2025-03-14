using HCore.Application.Modules.Roles.Dtos;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<bool> UpdateRoleAsync(int id, CreateRoleDto dto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
