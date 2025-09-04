using HCore.Application.Modules.Auth.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HCore.Infrastructure.Identity
{
    public class UserAuthManager : IUserAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public string? UserName =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public async Task<User?> FindByUsernameAsync(string userName)
            => await _userManager.FindByNameAsync(userName);

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id) ? null : await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
            => await _userManager.GetRolesAsync(user);

        public async Task<Role?> FindByRolenameAsync(string roleName)
            => await _roleManager.FindByNameAsync(roleName);

        public async Task<IList<Claim?>> GetClaimsByRoleAsync(Role role)
            => await _roleManager.GetClaimsAsync(role);
    }

}
