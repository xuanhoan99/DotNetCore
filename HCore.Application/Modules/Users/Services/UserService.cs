using AutoMapper;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCore.Application.Modules.Users.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<bool>> AssignRoles(string userId, List<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return BaseResponse<bool>.Fail("Không tìm thấy user.");

            roleNames ??= new List<string>();

            // Validate role tồn tại
            var invalid = new List<string>();
            foreach (var r in roleNames.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!await _roleManager.RoleExistsAsync(r))
                    invalid.Add(r);
            }
            if (invalid.Count > 0)
                return BaseResponse<bool>.Fail("Role không tồn tại: " + string.Join(", ", invalid));

            // Lấy roles hiện tại
            var current = await _userManager.GetRolesAsync(user);

            // Tính toAdd / toRemove
            var toAdd = roleNames.Except(current, StringComparer.OrdinalIgnoreCase).ToList();
            var toRemove = current.Except(roleNames, StringComparer.OrdinalIgnoreCase).ToList();

            if (toRemove.Count > 0)
            {
                var rmRes = await _userManager.RemoveFromRolesAsync(user, toRemove);
                if (!rmRes.Succeeded)
                    return BaseResponse<bool>.Fail("Bỏ role lỗi: " + string.Join("; ", rmRes.Errors.Select(e => e.Description)));
            }

            if (toAdd.Count > 0)
            {
                var addRes = await _userManager.AddToRolesAsync(user, toAdd);
                if (!addRes.Succeeded)
                    return BaseResponse<bool>.Fail("Gán role lỗi: " + string.Join("; ", addRes.Errors.Select(e => e.Description)));
            }

            return BaseResponse<bool>.Ok(true, "Cập nhật roles thành công.");
        }

        public async Task<BaseResponse<UserResponseDto>> Create(UserDto input)
        {
            var user = _mapper.Map<User>(input);
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<UserResponseDto>.Fail(string.Join("; ", errorMessages));
            }

            var userResponse = _mapper.Map<UserResponseDto>(user);
            return BaseResponse<UserResponseDto>.Ok(userResponse, "Create successfully");
        }

        public async Task<BaseResponse<bool>> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BaseResponse<bool>.Fail("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Fail(string.Join("; ", errors));
            }

            return BaseResponse<bool>.Ok(true, "Delete successfully");
        }

        public async Task<BaseResponse<List<UserResponseDto>>> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = _mapper.Map<List<UserResponseDto>>(users);
            return BaseResponse<List<UserResponseDto>>.Ok(result, "Get all users successfully");
        }

        public async Task<BaseResponse<UserResponseDto>> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BaseResponse<UserResponseDto>.Fail("User not found");

            var result = _mapper.Map<UserResponseDto>(user);
            return BaseResponse<UserResponseDto>.Ok(result, "Get user successfully");
        }

        public async Task<BaseResponse<List<string>>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return BaseResponse<List<string>>.Fail("Không tìm thấy user.");

            var roles = await _userManager.GetRolesAsync(user);
            return BaseResponse<List<string>>.Ok(roles.ToList());
        }

        public async Task<BaseResponse<UserResponseDto>> Update(string id, UserDto input)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BaseResponse<UserResponseDto>.Fail("User not found");

            _mapper.Map(input, user); // ánh xạ các trường từ input sang entity

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BaseResponse<UserResponseDto>.Fail("Update failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            var updatedUser = _mapper.Map<UserResponseDto>(user);
            return BaseResponse<UserResponseDto>.Ok(updatedUser, "Update successfully");
        }
    }
}
