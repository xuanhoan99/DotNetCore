using AutoMapper;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCore.Application.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
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
    }
}
