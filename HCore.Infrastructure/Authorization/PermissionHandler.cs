using HCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace HCore.Infrastructure.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMemoryCache _cache;

        public PermissionHandler(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cache = cache;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Lấy userId từ claim NameIdentifier
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            // Cache key để tránh query liên tục
            var cacheKey = $"perm:uid:{userId}";
            var perms = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                // 1. Lấy user object
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return new List<string>();

                // 2. Lấy role list
                var roles = await _userManager.GetRolesAsync(user); // danh sách tên role
                var permsFromRoles = new List<string>();
                foreach (var roleName in roles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role == null) continue;

                    // Lấy claim của role
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    permsFromRoles.AddRange(roleClaims
                        .Where(c => c.Type == "Permission")
                        .Select(c => c.Value));
                }

                return permsFromRoles;
            });

            // Kiểm tra permission
            if (perms.Contains(requirement.PermissionName))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
