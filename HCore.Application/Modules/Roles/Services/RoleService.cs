using AutoMapper;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Helpers;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.SysMenus.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HCore.Application.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ISysMenuService _sysMenuService;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<Role> roleManager, ISysMenuService sysMenuService, IMapper mapper)
        {
            _roleManager = roleManager;
            _sysMenuService = sysMenuService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<RoleOuputDto>> Create(RoleInsInputDto input)
        {
            var role = _mapper.Map<Role>(input);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<RoleOuputDto>.Fail(string.Join("; ", errorMessages));
            }

            var roleResponse = _mapper.Map<RoleOuputDto>(role);
            return BaseResponse<RoleOuputDto>.Ok(roleResponse, "Create successfully");
        }

        public async Task<BaseResponse<bool>> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return BaseResponse<bool>.Fail("Role not found");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Fail(string.Join("; ", errors));
            }

            return BaseResponse<bool>.Ok(true, "Delete successfully");

        }

        public async Task<BaseResponse<List<RoleOuputDto>>> GetAllRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var result = _mapper.Map<List<RoleOuputDto>>(roles);
            return BaseResponse<List<RoleOuputDto>>.Ok(result, "Get all roles successfully");
        }

        public async Task<BaseResponse<RoleOuputDto>> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return BaseResponse<RoleOuputDto>.Fail("Role not found");

            var result = _mapper.Map<RoleOuputDto>(role);
            return BaseResponse<RoleOuputDto>.Ok(result, "Get role successfully");
        }

        public async Task<BaseResponse<RoleOuputDto>> Update(string id, RoleInputDto input)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return BaseResponse<RoleOuputDto>.Fail("Role not found");

            _mapper.Map(input, role); // ánh xạ các trường từ input sang entity

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return BaseResponse<RoleOuputDto>.Fail("Update failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            var updatedRole = _mapper.Map<RoleOuputDto>(role);
            return BaseResponse<RoleOuputDto>.Ok(updatedRole, "Update successfully");
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
