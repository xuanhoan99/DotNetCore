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

            // Dùng từ điển để tra nhanh menu cha
            var appDict = appMenu.ToDictionary(x => x.Id);
            // Dùng set để tránh add trùng Value
            var existed = new HashSet<string>(allPermissions.Select(x => x.Value));

            foreach (var item in appMenu)
            {
                var permissionName = item.PermissionName;

                // Xác định Parent cho claim của chính menu này
                string? parentPermission =
                    item.ParentId.HasValue && appDict.TryGetValue(item.ParentId.Value, out var parentMenu)
                        ? parentMenu.PermissionName
                        : null; // nếu không có cha thì gán rootName (nếu có), else null

                // 1) Claim ở level menu (không action)
                if (!string.IsNullOrWhiteSpace(permissionName) && existed.Add(permissionName))
                {
                    allPermissions.Add(new RoleClaimsDto
                    {
                        Type = "Permission",
                        Value = permissionName,
                        Selected = false,
                        MenuName = item.Name, // hiển thị tên menu
                        Parent = parentPermission  // cha là menu cha hoặc rootName
                    });
                }

                // 2) Nếu là menu con thì sinh thêm các action: <PermissionName>.<action>
                if (item.ParentId.HasValue)
                {
                    foreach (var action in actions ?? Enumerable.Empty<string>())
                    {
                        if (string.IsNullOrWhiteSpace(action)) continue;

                        var actionValue = $"{permissionName}.{action}";
                        if (existed.Add(actionValue))
                        {
                            allPermissions.Add(new RoleClaimsDto
                            {
                                Type = "Permission",
                                Value = actionValue,
                                MenuName = item.Name,
                                // Cha của action là chính menu hiện tại (giống cách bạn set IsRootAction cho node)
                                Parent = permissionName
                            });
                        }
                    }
                }
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
