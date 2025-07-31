using HCore.Domain.Entities;
using System.Security.Claims;

namespace HCore.Application.Modules.Auth.Interfaces
{
    public interface IUserAuthManager
    {
        Task<User?> FindByUsernameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<User?> GetCurrentUserAsync();
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<Role?> FindByRolenameAsync(string roleName);
        Task<IList<Claim?>> GetClaimsByRoleAsync(Role role);
        string? UserName { get; }
    }

}
