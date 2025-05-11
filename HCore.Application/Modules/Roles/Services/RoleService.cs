using AutoMapper.Internal.Mappers;
using HCore.Application.Modules.Common;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Application.Modules.SysMenus.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security;

namespace HCore.Application.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ISysMenuService _sysMenuService;
        public RoleService(RoleManager<Role> roleManager,ISysMenuService sysMenuService)
        {
            _roleManager = roleManager;
            _sysMenuService = sysMenuService;
        }

        public async Task<RoleOuputDto> RoleById(string id)
        {
            RoleOuputDto roleOuputDto;
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                roleOuputDto = new RoleOuputDto();
            }
            else
            {
                roleOuputDto = new RoleOuputDto()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Desc = role.Desc
                };
            }
            return roleOuputDto;
        }

        public async Task<IdentityResult> RoleDel(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return await _roleManager.DeleteAsync(role);

        }

        public async Task<RoleOuputDto> RoleIns(RoleInsInputDto input)
        {
            var role = new Role()
            {
                Name = input.Name,
                NormalizedName = input.Name.ToUpper(),
                Desc = input.Desc
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return null;
            }

            var roleOuputDto = new RoleOuputDto
            {
                Id = role.Id,
                Name = role.Name,
                Desc = input.Desc
            };

            return roleOuputDto;
        }

        public async Task<List<RoleOuputDto>> GetAllRole()
        {
            var roles = await _roleManager.Roles.Select(s => new RoleOuputDto()
            {
                Id = s.Id,
                Desc = s.Desc,
                Name = s.Name
            }).ToListAsync();
            return roles;
        }

        public async Task<IdentityResult> RoleUpd(string id, RoleInputDto input)
        {
            var role = await _roleManager.FindByIdAsync(id);
            role.Name = input.Name;
            role.NormalizedName = input.Name.ToUpper();
            role.Desc = input.Desc;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<BaseResponse<RoloPermissionDto>> GetPermissionById(string roleId)
        {
            var result = new RoloPermissionDto();
            var actions = typeof(HCorePermissions.Action)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(null))
                .Select(x => x.ToString())
                .OrderBy(x => x.ToString())
                .ToList();

            var appMenu = _sysMenuService.GetAll();

            var allPermissions = new List<RoleClaimsDto>();
            allPermissions.GetPermissions(appMenu.Result.Data, actions);

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return BaseResponse<RoloPermissionDto>.Fail("Không tìm thấy role");
            result.RoleId = roleId;

            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            result.RoleClaims = allPermissions;
            return BaseResponse<RoloPermissionDto>.Ok(result, "Lấy danh sách permision thành công");
        }
        public async Task<BaseResponse<RoloPermissionDto>> UpdatePermision(RoloPermissionDto input)
        {
            var role = await _roleManager.FindByIdAsync(input.RoleId);
            if (role == null)
                return BaseResponse<RoloPermissionDto>.Fail("Không tìm thấy vai trò");

            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                    return BaseResponse<RoloPermissionDto>.Fail("Không thể xoá claim: " + claim.Type);
            }
            var selectedClaims = input.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return BaseResponse<RoloPermissionDto>.Ok(input, "Cập nhật permision thành công");

        }
    }
}
