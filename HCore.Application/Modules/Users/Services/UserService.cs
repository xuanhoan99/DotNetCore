using AutoMapper;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HCore.Application.Modules.Users.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public string? UserName =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
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
