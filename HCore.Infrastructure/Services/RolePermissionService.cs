using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Helpers;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Permissions.Dtos;
using HCore.Application.Modules.Permissions.Interfaces;
using HCore.Application.Modules.SysMenus.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace HCore.Infrastructure.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ISysMenuService _sysMenuService;

        public RolePermissionService(RoleManager<Role> roleManager, ISysMenuService sysMenuService)
        {
            _roleManager = roleManager;
            _sysMenuService = sysMenuService;
        }
        public async Task<BaseResponse<RolePermissionDto>> UpdatePermision(RolePermissionDto input)
        {
            var role = await _roleManager.FindByIdAsync(input.RoleId);
            if (role == null)
                return BaseResponse<RolePermissionDto>.Fail("Không tìm thấy vai trò");

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in existingClaims)
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                    return BaseResponse<RolePermissionDto>.Fail("Không thể xoá claim: " + claim.Type);
            }
            var selectedClaims = input.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claimDto in selectedClaims)
            {
                var addResult = await _roleManager.AddPermissionClaim(role, claimDto.Type, claimDto.Value);
                if (!addResult.Succeeded)
                {
                    return BaseResponse<RolePermissionDto>.Fail(
                        $"Không thể thêm claim: {string.Join(';', addResult.Errors.Select(e => e.Description))}");
                }
            }
            return BaseResponse<RolePermissionDto>.Ok(input, "Cập nhật permision thành công");

        }
        public async Task<BaseResponse<RolePermissionDto>> GetPermissionById(string roleId)
        {
            // todo có thể cache phần "all" vì menu/actions ít đổi
            var allPermsResult = await BuildAllPermissionsAsync();

            if (allPermsResult is null || !allPermsResult.Success)
                return BaseResponse<RolePermissionDto>.Fail(allPermsResult?.Message ?? "Không dựng được permissions");

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return BaseResponse<RolePermissionDto>.Fail("Không tìm thấy role");

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var roleClaimValues = new HashSet<string>(
                   roleClaims
                       .Where(c => c.Type == "Permission")
                       .Select(c => c.Value)
               );

            // gán Selected theo role
            foreach (var perm in allPermsResult.Data)
                perm.Selected = roleClaimValues.Contains(perm.Value);

            var result = new RolePermissionDto
            {
                RoleId = roleId,
                RoleClaims = allPermsResult.Data
            };
            return BaseResponse<RolePermissionDto>.Ok(result, "Lấy danh sách permission thành công");
        }

        public async Task<BaseResponse<RolePermissionDto>> GetPermissionAll()
        {
            // cũng có thể cache
            var allPermsResult = await BuildAllPermissionsAsync();

            if (allPermsResult is null || !allPermsResult.Success)
                return BaseResponse<RolePermissionDto>.Fail(allPermsResult?.Message ?? "Không dựng được permissions");

            var result = new RolePermissionDto
            {
                RoleId = null, // hoặc "", tùy DTO của bạn
                RoleClaims = allPermsResult.Data
            };
            return BaseResponse<RolePermissionDto>.Ok(result, "Lấy toàn bộ permission thành công");
        }

        private static List<string> GetActions()
        {
            return typeof(HCorePermissions.Action)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(null)?.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .ToList()!;
        }

        private async Task<BaseResponse<List<RoleClaimsDto>>> BuildAllPermissionsAsync()
        {
            var actions = GetActions();

            var menuResult = await _sysMenuService.GetAll();
            if (!menuResult.Success)
                return BaseResponse<List<RoleClaimsDto>>.Fail("Không lấy được danh sách menu");

            var allPermissions = new List<RoleClaimsDto>();
            allPermissions.GetPermissions(menuResult.Data, actions); // dùng extension của bạn

            // đảm bảo mặc định Selected=false
            foreach (var p in allPermissions) p.Selected = false;

            return BaseResponse<List<RoleClaimsDto>>.Ok(allPermissions);
        }
    }
}
