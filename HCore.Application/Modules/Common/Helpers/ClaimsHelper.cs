using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Permissions.Dtos;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HCore.Application.Modules.Common.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RoleClaimsDto> allPermissions, List<SysMenuDto> appMenu, List<string> actions)
        {
            foreach (var item in appMenu)
            {
                var permissionName = $"{HCorePermissions.Prefix.Main}.{item.EnglishName}";
                var permissions = item.ParentId == null
                            ? new[] { permissionName }
                            : actions.Select(action => $"{permissionName}.{action}").Append(permissionName);

                allPermissions.AddRange(permissions.Select(p => new RoleClaimsDto
                {
                    Value = p,
                    Type = "Permission"
                }));
            }
        }

        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string claimType, string claimValue)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (allClaims.Any(c => c.Type == claimType && c.Value == claimValue))
            {
                return IdentityResult.Success;
            }
            return await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));
        }
    }
}
