using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Common
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
                            : actions.Select(action => $"{permissionName}.{action}");

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
