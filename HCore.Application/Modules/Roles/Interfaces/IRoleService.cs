using HCore.Application.Modules.Roles.Dtos;

namespace HCore.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
        Task<RoleDto> GetRoleByIdAsync(Guid id);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<bool> UpdateRoleAsync(Guid id, CreateRoleDto dto);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}
