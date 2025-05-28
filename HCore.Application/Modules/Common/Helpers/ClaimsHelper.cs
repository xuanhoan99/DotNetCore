using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Roles.Dtos;
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
                    Type = "Permissions"
                }));
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
